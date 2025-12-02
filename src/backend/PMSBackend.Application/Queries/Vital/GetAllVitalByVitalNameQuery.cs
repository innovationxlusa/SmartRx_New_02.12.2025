using MediatR;
using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Queries.Vital
{
    public class GetAllVitalByVitalNameQuery : IRequest<List<VitalDTO>>
    {
        public long? Id { get; set; }
        public string VitalName { get; set; }


    }
    public class GetAllVitalByVitalNameQueryHandler : IRequestHandler<GetAllVitalByVitalNameQuery, List<VitalDTO>>
    {
        private readonly IVitalRepository _vitalRepository;
        public GetAllVitalByVitalNameQueryHandler(IVitalRepository vitalRepository)
        {
            _vitalRepository = vitalRepository;
        }
        public async Task<List<VitalDTO>> Handle(GetAllVitalByVitalNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                VitalDTO vitalDto = new VitalDTO();
                List<VitalDTO> vitalList = new List<VitalDTO>();
                var vitals = await _vitalRepository.GetVitalByName(request.VitalName);
                if (vitals != null)
                {
                    Parallel.ForEach(vitals, vital =>
                    {
                        vitalDto = new VitalDTO()
                        {
                            Id = vital.Id,
                            Code = vital.Code,
                            Name = vital.Name,
                            ApplicableEntity = vital.ApplicableEntity,
                            Description = vital.Description,
                            UnitMeasurementUnit = vital.Unit.MeasurementUnit,
                            UnitCode = vital.Unit.Code,
                            UnitDescription = vital.Unit.Description,
                            UnitDetails = vital.Unit.Details,
                            UnitId = vital.Unit.Id,
                            UnitName = vital.Unit.Name,
                            UnitType = vital.Unit.Type,
                        };
                        vitalList.Add(vitalDto);
                    });
                }
                await Task.CompletedTask;
                return vitalList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}