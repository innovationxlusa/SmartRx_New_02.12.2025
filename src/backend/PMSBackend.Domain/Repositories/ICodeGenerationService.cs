using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Domain.Repositories
{
    public interface ICodeGenerationService
    {
        Task<string> GeneratePatientCodeAsync(CancellationToken cancellationToken = default);
        Task<string> GenerateDoctorCodeAsync(CancellationToken cancellationToken = default);
        Task<string> GenerateRewardCodeAsync(CancellationToken cancellationToken = default);
        Task<string> GenerateUserCodeAsync(CancellationToken cancellationToken = default);
        Task<string> GenerateUserActivityCodeAsync(CancellationToken cancellationToken = default);
        Task<string> GenerateCodeAsync(string prefix, Func<IQueryable<string>, IQueryable<string>> queryBuilder, int digitLength, CancellationToken cancellationToken = default);
    }
}
