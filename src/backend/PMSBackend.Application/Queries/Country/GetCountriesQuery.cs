using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Queries.Country
{
    public class GetCountriesQuery : IRequest<List<CountryDTO>>
    {
    }

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDTO>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<CountryDTO>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var countries = await _countryRepository.GetAllCountriesAsync();
                
                return countries.Select(c => new CountryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    CountryCode = c.CountryCode
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving countries: {ex.Message}");
            }
        }
    }
}
