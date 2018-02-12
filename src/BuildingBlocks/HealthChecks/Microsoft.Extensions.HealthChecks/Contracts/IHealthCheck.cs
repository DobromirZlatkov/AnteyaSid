using System.Threading;
using System.Threading.Tasks;

namespace AnteyaSidOnContainers.BuildingBlocks.HealthChecks.Microsoft.Extensions.HealthChecks.Contracts
{
    public interface IHealthCheck
    {
        ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
