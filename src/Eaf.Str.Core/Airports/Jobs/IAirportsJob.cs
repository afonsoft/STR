using Eaf.BackgroundJobs.Hangfire;
using Eaf.Dependency;
using Hangfire;

namespace Eaf.Str.Airports.Jobs
{
    [DisableConcurrentExecution(300)]
    public interface IAirportsJob : IAsyncBackgroundJob<bool>, ITransientDependency
    {
    }
}