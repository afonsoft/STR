using Eaf.BackgroundJobs.Hangfire;
using Eaf.Timing;
using Hangfire.Server;
using Hangfire.Console;
using System.Threading;
using System.Threading.Tasks;

namespace Eaf.Str.Airplanes.Jobs
{
    public class AirplaneJob : AsyncBackgroundJob<string>, IAirplaneJob
    {
        private readonly BackgroundJobs.IBackgroundJobManager _backgroundJobManager;

        public AirplaneJob(
            BackgroundJobs.IBackgroundJobManager backgroundJobManager
        )
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public override Task ExecuteAsync(string args, PerformContext context, CancellationToken token)
        {
            context.WriteLine("Start Job");
            context.WriteLine($"Pint args: {args}");
            context.WriteLine("End Job");
            return Task.CompletedTask;
        }

        public Task StartProcess()
        {
            return _backgroundJobManager.EnqueueAsync<AirplaneJob, string>($"Test Job Args {Clock.Now:dd/MM/yyyy}");
        }
    }
}