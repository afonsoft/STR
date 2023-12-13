using Eaf.BackgroundJobs.Hangfire;
using Eaf.Dependency;
using System.Threading.Tasks;

namespace Eaf.Str.Airports.Jobs
{
    public interface IAirportsJob : IAsyncBackgroundJob<bool>, ITransientDependency
    {
    }
}