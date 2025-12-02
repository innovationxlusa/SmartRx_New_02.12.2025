using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Domain.CommonDTO;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using PMSBackend.Domain.SharedContract;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.PatientReward
{
    public class GetPatientRewardsByUserIdAndPatientIdQuery : IRequest<PatientRewardSplitResult>
    {
        public long UserId { get; set; }
        public long? PatientId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool? Earned { get; set; } = null; // null means show all (both earned and consumed)

        public bool? Consumed { get; set; } = null; // null means show all (both earned and consumed)
        public PagingSortingParams? PagingSorting { get; set; } = new PagingSortingParams
        {
            PageNumber = 1,
            PageSize = 10,
            SortBy = "CreatedDate",
            SortDirection = "desc"
        };

    }

    public class GetPatientRewardsByUserIdAndPatientIdQueryHandler
        : IRequestHandler<GetPatientRewardsByUserIdAndPatientIdQuery, PatientRewardSplitResult>
    {
        private readonly IRewardTransactionRepository _rewardTransactionRepository;
        private readonly IRewardPointConversionsRepository _rewardPointConversionsRepository;

        public GetPatientRewardsByUserIdAndPatientIdQueryHandler(
            IRewardTransactionRepository rewardTransactionRepository,
            IRewardPointConversionsRepository rewardPointConversionsRepository)
        {
            _rewardTransactionRepository = rewardTransactionRepository;
            _rewardPointConversionsRepository = rewardPointConversionsRepository;
        }

        public async Task<PatientRewardSplitResult> Handle(
            GetPatientRewardsByUserIdAndPatientIdQuery request,
            CancellationToken cancellationToken)
        {
            // Get IQueryable queries for all, earned, and consumed transactions
            var transactionQueries = _rewardTransactionRepository.GetRewardTransactionQueriesByUserId(
                request.UserId,
                request.PatientId,
                request.StartDate,
                request.EndDate);

            // Map IQueryable<SmartRx_RewardTransactionEntity> to PatientRewardContract
            var mapToContract = (IQueryable<SmartRx_RewardTransactionEntity> query) => query
                .Select(tr => new PatientRewardContract
                {
                    Id = tr.Id,
                    SmartRxMasterId = tr.SmartRxMasterId,
                    PrescriptionId = tr.PrescriptionId,
                    PatientId = tr.PatientId ?? request.PatientId ?? 0,
                    RewardId = tr.RewardRuleId,
                    RewardName = tr.RewardRule != null ? (tr.RewardRule.ActivityTaken) : null,
                    BadgeId = tr.BadgeId ?? 0,
                    BadgeName = tr.RewardBadge != null ? tr.RewardBadge.Name : null,
                    BadgeDescription = tr.RewardBadge != null ? tr.RewardBadge.Description : null,
                    PatientFirstName = null,
                    PatientLastName = null,
                    PatientCode = null,
                    IsDeduct=tr.RewardRule.IsDeductible,
                    // Map points based on RewardRule.IsDeductible
                    EarnedNonCashablePoints = (tr.RewardRule == null || !tr.RewardRule.IsDeductible) && tr.NonCashableBalance != 0 && tr.NonCashableBalance > 0
                        ? tr.NonCashableBalance : 0,
                    ConsumedNonCashablePoints = (tr.RewardRule != null && tr.RewardRule.IsDeductible) && tr.NonCashableBalance != 0 && tr.NonCashableBalance < 0
                        ? Math.Abs(tr.NonCashableBalance) : 0,
                    ConvertedNonCashableToCashablePoints = 0,
                    TotalNonCashablePoints = tr.NonCashableBalance,
                    EarnedCashablePoints = (tr.RewardRule == null || !tr.RewardRule.IsDeductible) && tr.CashableBalance != 0 && tr.CashableBalance > 0
                        ? tr.CashableBalance : 0,
                    ConsumedCashablePoints = (tr.RewardRule != null && tr.RewardRule.IsDeductible) && tr.CashableBalance != 0 && tr.CashableBalance < 0
                        ? Math.Abs(tr.CashableBalance) : 0,
                    ConvertedCashableToNonCashablePoints = 0,
                    TotalCashablePoints = tr.CashableBalance,
                    ConvertedCashableToMoney = (tr.RewardRule == null || !tr.RewardRule.IsDeductible) && tr.CashedMoneyBalance != 0 && tr.CashedMoneyBalance > 0
                        ? tr.CashedMoneyBalance : 0,
                    EncashedMoney = (tr.RewardRule != null && tr.RewardRule.IsDeductible) && tr.CashedMoneyBalance != 0 && tr.CashedMoneyBalance < 0
                        ? Math.Abs(tr.CashedMoneyBalance) : 0,
                    TotalMoney = tr.CashedMoneyBalance,
                    // Map unified TotalPoints depending on the reward type
                    TotalPoints =
                        ((tr.RewardRule == null || !tr.RewardRule.IsDeductible) && tr.NonCashableBalance != 0 && tr.NonCashableBalance > 0) ? tr.NonCashableBalance
                        : ((tr.RewardRule == null || !tr.RewardRule.IsDeductible) && tr.CashableBalance != 0 && tr.CashableBalance > 0) ? tr.CashableBalance
                        : (tr.CashedMoneyBalance != 0 && tr.CashedMoneyBalance > 0) ? tr.CashedMoneyBalance : 0,
                    Remarks = tr.Remarks,
                    CreatedDate = tr.CreatedDate,
                    ModifiedDate = tr.ModifiedDate
                });

            // Get all contracts as IQueryable
            var allContractsQuery = mapToContract(transactionQueries.All);
            var earnedContractsQuery = mapToContract(transactionQueries.Earned);
            var consumedContractsQuery = mapToContract(transactionQueries.Consumed);

            // Get total counts before pagination
            var allCount = await allContractsQuery.CountAsync(cancellationToken);
            var earnedCount = await earnedContractsQuery.CountAsync(cancellationToken);
            var consumedCount = await consumedContractsQuery.CountAsync(cancellationToken);

            // Get conversion data from SmartRx_RewardPointConversion table
            var conversions = await _rewardPointConversionsRepository.GetByUserIdAsync(request.UserId);

            // Create conversion contracts for cashable conversions (ToType == Cashable)
            var cashableConversions = conversions
                .Where(c => c.ToType == RewardType.Cashable)
                .Select(c => new PatientRewardContract
                {
                    Id = c.Id,
                    SmartRxMasterId = null,
                    PrescriptionId = null,
                    PatientId = request.PatientId ?? 0,
                    RewardId = 0,
                    RewardName = "Conversion to Cashable",
                    BadgeId = 0,
                    BadgeName = "Conversion",
                    BadgeDescription = $"Converted from {((RewardType)c.FromType).ToString()} to Cashable",
                    PatientFirstName = null,
                    PatientLastName = null,
                    PatientCode = null,
                    EarnedNonCashablePoints = 0,
                    ConsumedNonCashablePoints = 0,
                    ConvertedNonCashableToCashablePoints = 0,
                    TotalNonCashablePoints = 0,
                    EarnedCashablePoints = (int)c.ConvertedPoints,
                    ConsumedCashablePoints = 0,
                    ConvertedCashableToNonCashablePoints = 0,
                    TotalCashablePoints = (int)c.ConvertedPoints,
                    ConvertedCashableToMoney = 0,
                    EncashedMoney = 0,
                    TotalMoney = 0,
                    TotalPoints = c.ConvertedPoints,
                    Remarks = $"Conversion: {c.ConvertedPoints} points converted to Cashable",
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate
                }).ToList();

            // Create conversion contracts for noncashable conversions (ToType == NonCashable)
            var nonCashableConversions = conversions
                .Where(c => c.ToType == RewardType.Noncashable)
                .Select(c => new PatientRewardContract
                {
                    Id = c.Id,
                    SmartRxMasterId = null,
                    PrescriptionId = null,
                    PatientId = request.PatientId ?? 0,
                    RewardId = 0,
                    RewardName = "Conversion to NonCashable",
                    BadgeId = 0,
                    BadgeName = "Conversion",
                    BadgeDescription = $"Converted from {((RewardType)c.FromType).ToString()} to NonCashable",
                    PatientFirstName = null,
                    PatientLastName = null,
                    PatientCode = null,
                    EarnedNonCashablePoints = (int)c.ConvertedPoints,
                    ConsumedNonCashablePoints = 0,
                    ConvertedNonCashableToCashablePoints = 0,
                    TotalNonCashablePoints = (int)c.ConvertedPoints,
                    EarnedCashablePoints = 0,
                    ConsumedCashablePoints = 0,
                    ConvertedCashableToNonCashablePoints = 0,
                    TotalCashablePoints = 0,
                    ConvertedCashableToMoney = 0,
                    EncashedMoney = 0,
                    TotalMoney = 0,
                    TotalPoints = (int)c.ConvertedPoints,
                    Remarks = $"Conversion: {c.ConvertedPoints} points converted to NonCashable",
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate
                }).ToList();

            // Get contracts from queries
            var allContracts = await allContractsQuery.ToListAsync(cancellationToken);
            var earnedContracts = await earnedContractsQuery.ToListAsync(cancellationToken);
            var consumedContracts = await consumedContractsQuery.ToListAsync(cancellationToken);

            // Get original cashable and noncashable lists
            var originalCashable = earnedContracts.Where(c => c.EarnedCashablePoints > 0).ToList();
            var originalNonCashable = earnedContracts.Where(c => c.EarnedNonCashablePoints > 0).ToList();

            // Concatenate conversion data to the lists
            var finalEarnedCashable = originalCashable.Concat(cashableConversions).ToList();
            var finalEarnedNonCashable = originalNonCashable.Concat(nonCashableConversions).ToList();
            var totalEarnedList = finalEarnedCashable.Concat(finalEarnedNonCashable).ToList();


            // Create conversion contracts for consumed conversions (FromType indicates what was consumed)
            var cashableConsumed = conversions
                .Where(c => c.FromType == RewardType.Cashable)
                .Select(c => new PatientRewardContract
                {
                    Id = c.Id,
                    SmartRxMasterId = null,
                    PrescriptionId = null,
                    PatientId = request.PatientId ?? 0,
                    RewardId = 0,
                    RewardName = "Conversion from Cashable",
                    BadgeId = 0,
                    BadgeName = "Conversion",
                    BadgeDescription = $"Converted from Cashable to {((RewardType)c.ToType).ToString()}",
                    PatientFirstName = null,
                    PatientLastName = null,
                    PatientCode = null,
                    EarnedNonCashablePoints = 0,
                    ConsumedNonCashablePoints = 0,
                    ConvertedNonCashableToCashablePoints = 0,
                    TotalNonCashablePoints = 0,
                    EarnedCashablePoints = 0,
                    ConsumedCashablePoints = (int)c.ConvertedPoints,
                    ConvertedCashableToNonCashablePoints = 0,
                    TotalCashablePoints = -(int)c.ConvertedPoints,
                    ConvertedCashableToMoney = 0,
                    EncashedMoney = 0,
                    TotalMoney = 0,
                    TotalPoints = -c.ConvertedPoints,
                    Remarks = $"Conversion: {c.ConvertedPoints} cashable points consumed",
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate
                }).ToList();

            // Create conversion contracts for noncashable consumed conversions
            var nonCashableConsumed = conversions
                .Where(c => c.FromType == RewardType.Noncashable)
                .Select(c => new PatientRewardContract
                {
                    Id = c.Id,
                    SmartRxMasterId = null,
                    PrescriptionId = null,
                    PatientId = request.PatientId ?? 0,
                    RewardId = 0,
                    RewardName = "Conversion from NonCashable",
                    BadgeId = 0,
                    BadgeName = "Conversion",
                    BadgeDescription = $"Converted from NonCashable to {((RewardType)c.ToType).ToString()}",
                    PatientFirstName = null,
                    PatientLastName = null,
                    PatientCode = null,
                    EarnedNonCashablePoints = 0,
                    ConsumedNonCashablePoints = (int)c.ConvertedPoints,
                    ConvertedNonCashableToCashablePoints = 0,
                    TotalNonCashablePoints = -(int)c.ConvertedPoints,
                    EarnedCashablePoints = 0,
                    ConsumedCashablePoints = 0,
                    ConvertedCashableToNonCashablePoints = 0,
                    TotalCashablePoints = 0,
                    ConvertedCashableToMoney = 0,
                    EncashedMoney = 0,
                    TotalMoney = 0,
                    TotalPoints = -(int)c.ConvertedPoints,
                    Remarks = $"Conversion: {c.ConvertedPoints} noncashable points consumed",
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate
                }).ToList();
            
            var totalConsumedList = consumedContracts.Concat(cashableConsumed).Concat(nonCashableConsumed).ToList();

            // Combine all data including conversions
            var allData = allContracts.Concat(cashableConversions).Concat(nonCashableConversions).ToList();

            // Apply pagination manually
            var pageNumber = request.PagingSorting?.PageNumber ?? 1;
            var pageSize = request.PagingSorting?.PageSize ?? 10;
            var sortBy = request.PagingSorting?.SortBy ?? "CreatedDate";
            var sortDirection = request.PagingSorting?.SortDirection ?? "desc";

            // Apply sorting
            IOrderedEnumerable<PatientRewardContract> sortedAllData;
            IOrderedEnumerable<PatientRewardContract> sortedEarnedList;
            IOrderedEnumerable<PatientRewardContract> sortedConsumedList;

            if (sortDirection?.ToLower() == "asc")
            {
                sortedAllData = allData.OrderBy(x => GetPropertyValue(x, sortBy));
                sortedEarnedList = totalEarnedList.OrderBy(x => GetPropertyValue(x, sortBy));
                sortedConsumedList = totalConsumedList.OrderBy(x => GetPropertyValue(x, sortBy));
            }
            else
            {
                sortedAllData = allData.OrderByDescending(x => GetPropertyValue(x, sortBy));
                sortedEarnedList = totalEarnedList.OrderByDescending(x => GetPropertyValue(x, sortBy));
                sortedConsumedList = totalConsumedList.OrderByDescending(x => GetPropertyValue(x, sortBy));
            }

            // Apply pagination
            var pagedAllData = sortedAllData
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedEarnedList = sortedEarnedList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedConsumedList = sortedConsumedList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PatientRewardSplitResult
            {
                All = new PaginatedResult<PatientRewardContract>(
                    pagedAllData,
                    allData.Count,
                    pageNumber,
                    pageSize,
                    sortBy,
                    sortDirection,
                    null, null, null, null, null),
                Earned = new PaginatedResult<PatientRewardContract>(
                    pagedEarnedList,
                    totalEarnedList.Count,
                    pageNumber,
                    pageSize,
                    sortBy,
                    sortDirection,
                    null, null, null, null, null),
                Consumed = new PaginatedResult<PatientRewardContract>(
                    pagedConsumedList,
                    totalConsumedList.Count,
                    pageNumber,
                    pageSize,
                    sortBy,
                    sortDirection,
                    null, null, null, null, null)
            };
        }

        private object GetPropertyValue(PatientRewardContract obj, string propertyName)
        {
            var property = typeof(PatientRewardContract).GetProperty(propertyName);
            return property?.GetValue(obj) ?? obj.CreatedDate ?? DateTime.MinValue;
        }
    }
}


