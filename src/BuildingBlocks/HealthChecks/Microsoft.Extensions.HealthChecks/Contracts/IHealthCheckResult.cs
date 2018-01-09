using System.Collections.Generic;

namespace AnteyaSidOnContainers.BuildingBlocks.HealthChecks.Microsoft.Extensions.HealthChecks.Contracts
{
    public interface IHealthCheckResult
    {
        CheckStatus CheckStatus { get; }
        string Description { get; }
        IReadOnlyDictionary<string, object> Data { get; }
    }
}
