using Eaf.Application.Features;
using Eaf.Configuration;
using Eaf.Middleware.Authorization.Roles;
using Eaf.Middleware.Authorization.Users;
using Eaf.Middleware.MultiTenancy;
using Eaf.Str.Airplanes;
using Eaf.Str.Airports;
using Eaf.Str.AWBs;
using System;
using System.Linq;

namespace Eaf.Str.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public static readonly Type[] StrTrackedTypes =
        {
            typeof(Role),
            typeof(Tenant),
            typeof(User),
            typeof(Setting),
            typeof(FeatureSetting),
            typeof(Airplane),
            typeof(AwbAddress),
            typeof(AwbItem),
            typeof(Awb),
            typeof(Tracking),
            typeof(Airport)
        };

        public static Type[] TrackedTypes { get; } = StrTrackedTypes
            .GroupBy(type => type.FullName)
            .Select(types => types.First())
            .ToArray();
    }
}