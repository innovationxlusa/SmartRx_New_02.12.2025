using MediatR;
using Microsoft.Extensions.Logging;
using PMSBackend.Domain.Entities;
using PMSBackend.Domain.Repositories;

namespace PMSBackend.Application.Commands.PrescriptionUpload
{
    public class PrescriptionUploadSequenceGenerateCommand : IRequest<string>
    {
        public string SeqNo { get; set; }
    }
    public class PrescriptionUploadSequenceGenerateCommandHandler : IRequestHandler<PrescriptionUploadSequenceGenerateCommand, string>
    {

        private readonly IPrescriptionUploadRepository _repository;
        private readonly ILogger<PrescriptionUploadSequenceGenerateCommandHandler> _logger;
        public PrescriptionUploadSequenceGenerateCommandHandler(IPrescriptionUploadRepository repository, ILogger<PrescriptionUploadSequenceGenerateCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<string> Handle(PrescriptionUploadSequenceGenerateCommand request, CancellationToken cancellationToken)
        {
            Prescription_UploadEntity prescriptionLastInfo = new Prescription_UploadEntity();
            string newPrescriptionSequence = string.Empty;
            try
            {
                prescriptionLastInfo = await _repository.GetLastSavedPrescriptionCode();
                if (prescriptionLastInfo is null)
                    prescriptionLastInfo = new Prescription_UploadEntity();
                newPrescriptionSequence = (Convert.ToInt64(prescriptionLastInfo.PrescriptionCode) + 1).ToString().PadLeft(10, '0');
                // var seq = await _repository.GenerateFileSequenceAsync(newPrescriptionSequence);
                _logger.LogInformation($"New prescription Id generated: {newPrescriptionSequence}");
                await Task.CompletedTask;
                return newPrescriptionSequence;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"New prescription Id generation failed. Last Id is: {prescriptionLastInfo.PrescriptionCode}");
                throw;
            }

        }

    }
}
