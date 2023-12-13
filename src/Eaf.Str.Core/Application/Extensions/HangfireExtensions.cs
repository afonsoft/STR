using Eaf.BackgroundJobs;
using Eaf.Str.Airplanes;
using Eaf.Str.Airports.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using System;

namespace Eaf.Str.Application.Extensions
{
    public static class HangfireExtensions
    {
        public static void ScheduleRecurringJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<IAirplaneManager>("DateUpdateProcess", x => x.DateUpdate(null), Cron.Minutely, TimeZoneInfo.Local);
            var backgroundJobManager = app.ApplicationServices.GetService(typeof(IBackgroundJobManager)) as IBackgroundJobManager;
            backgroundJobManager.EnqueueAsync<AirportsJob, bool>(false);
        }
    }
}