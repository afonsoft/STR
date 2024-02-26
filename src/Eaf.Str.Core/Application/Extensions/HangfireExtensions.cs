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
            //RecurringJob.AddOrUpdate<IAirplaneManager>("DateUpdateProcess", x => x.DateUpdate(null), Cron.Yearly, TimeZoneInfo.Local);
            //RecurringJob.AddOrUpdate<IAirportsJob>("AirportsJob", x => x.ExecuteAsync(true, null, default), Cron.Yearly, TimeZoneInfo.Local);
        }
    }
}