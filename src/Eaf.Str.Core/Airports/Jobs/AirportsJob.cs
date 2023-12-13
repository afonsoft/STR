using Eaf.BackgroundJobs.Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eaf.Str.Airports.Jobs
{
    public class AirportsJob : AsyncBackgroundJob<bool>, IAirportsJob
    {
        public override Task ExecuteAsync(bool args, PerformContext context, CancellationToken token)
        {
            context.WriteLine("Start AirportsJob");
            context.WriteLine($"Force Update: {args}");
            context.WriteLine("End AirportsJob");
            return Task.CompletedTask;
        }
    }
}
}