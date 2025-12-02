using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.CommonServices;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.RewardPointConversions
{
    public class DeleteRewardPointConversionsCommand : IRequest<ApiResponseResult>
    {
        public RewardPointConversionsDeleteDTO DeleteDTO { get; set; } = null!;
    }

    public class DeleteRewardPointConversionsCommandHandler : IRequestHandler<DeleteRewardPointConversionsCommand, ApiResponseResult>
    {
        private readonly IRewardPointConversionsRepository _repository;

        public DeleteRewardPointConversionsCommandHandler(IRewardPointConversionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseResult> Handle(DeleteRewardPointConversionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _repository.ExistsAsync(request.DeleteDTO.Id);
                if (!exists)
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 404,
                        Status = "Failed",
                        Message = $"Reward point conversion with ID {request.DeleteDTO.Id} not found"
                    };
                }

                var result = await _repository.DeleteAsync(request.DeleteDTO.Id);
                
                if (result)
                {
                    return new ApiResponseResult
                    {
                        Data = result,
                        StatusCode = 200,
                        Status = "Success",
                        Message = "Reward point conversion deleted successfully"
                    };
                }
                else
                {
                    return new ApiResponseResult
                    {
                        Data = null,
                        StatusCode = 500,
                        Status = "Error",
                        Message = "Failed to delete reward point conversion"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while deleting reward point conversion: {ex.Message}"
                };
            }
        }
    }
}
