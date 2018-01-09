using AnteyaSidOnContainers.BuildingBlocks.HealthChecks.Microsoft.Extensions.HealthChecks.Contracts;
using System.Collections.Generic;

namespace AnteyaSidOnContainers.BuildingBlocks.HealthChecks.Microsoft.Extensions.HealthChecks
{
    public class HealthCheckResults
    {
        public IList<IHealthCheckResult> CheckResults { get; } = new List<IHealthCheckResult>();
    }
}
