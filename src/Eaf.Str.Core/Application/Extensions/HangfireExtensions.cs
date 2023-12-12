using Eaf.Str.Airplanes;
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
        }
    }
}