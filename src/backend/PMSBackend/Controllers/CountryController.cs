using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSBackend.Application.CommonServices;
using PMSBackend.Application.DTOs;
using PMSBackend.Application.Queries.Country;
using PMSBackend.Domain.SharedContract;
using System;
using System.Threading.Tasks;

namespace PMSBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all country endpoints
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns>List of countries</returns>
        [AllowAnonymous]
        [HttpGet("GetAllCountry")]
        [ProducesDefaultResponseType(typeof(List<CountryDTO>))]
        public async Task<IActionResult> GetCountries(CancellationToken cancellationToken = default)
        {
            try
            {
                var countries = await _mediator.Send(new GetCountriesQuery(), cancellationToken);

                return Ok(new ApiResponseResult
                {
                    Data = countries,
                    StatusCode = 200,
                    Status = "Success",
                    Message = "Countries retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseResult
                {
                    Data = null,
                    StatusCode = 500,
                    Status = "Error",
                    Message = $"An error occurred while retrieving countries: {ex.Message}"
                });
            }
        }
    }
}
