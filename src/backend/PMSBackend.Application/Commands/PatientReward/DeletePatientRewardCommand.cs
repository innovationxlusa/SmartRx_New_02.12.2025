using MediatR;
using PMSBackend.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMSBackend.Application.Commands.PatientReward
{
    public class DeletePatientRewardCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeletePatientRewardCommandHandler : IRequestHandler<DeletePatientRewardCommand, bool>
    {
        private readonly IPatientRewardRepository _patientRewardRepository;

        public DeletePatientRewardCommandHandler(IPatientRewardRepository patientRewardRepository)
        {
            _patientRewardRepository = patientRewardRepository;
        }

        public async Task<bool> Handle(DeletePatientRewardCommand request, CancellationToken cancellationToken)
        {
            return await _patientRewardRepository.DeletePatientRewardAsync(request.Id, cancellationToken);
        }
    }
}

