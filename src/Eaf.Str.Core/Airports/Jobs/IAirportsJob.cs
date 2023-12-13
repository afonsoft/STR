using Eaf.BackgroundJobs.Hangfire;
using Eaf.Dependency;
using Hangfire;
using System.ComponentModel;

namespace Eaf.Str.Airports.Jobs
{
    [DisableConcurrentExecution(300)]
    [Description("Update Airports")]
    public interface IAirportsJob : IAsyncBackgroundJob<bool>, ITransientDependency
    {
    }
}