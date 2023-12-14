using Eaf.Authorization;
using Eaf.Localization;
using Eaf.Middleware.Authorization;

namespace Eaf.Str.Authorization
{
    public class StrAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(MiddlewarePermissions.Pages) ?? context.CreatePermission(MiddlewarePermissions.Pages, LEaf("Pages"));

            var airplanes = pages.CreateChildPermission(StrPermissions.Pages_Airplanes, L("Airplanes"));
            airplanes.CreateChildPermission(StrPermissions.Pages_Airplanes_Create, L("CreateNewAirplane"));
            airplanes.CreateChildPermission(StrPermissions.Pages_Airplanes_Edit, L("EditAirplane"));
            airplanes.CreateChildPermission(StrPermissions.Pages_Airplanes_Delete, L("DeleteAirplane"));

            var airports = pages.CreateChildPermission(StrPermissions.Pages_Airports, L("Airports"));
            airports.CreateChildPermission(StrPermissions.Pages_Airports_Create, L("CreateNewAirports"));
            airports.CreateChildPermission(StrPermissions.Pages_Airports_Edit, L("EditAirports"));
            airports.CreateChildPermission(StrPermissions.Pages_Airports_Delete, L("DeleteAirports"));

            var tracking = pages.CreateChildPermission(StrPermissions.Pages_Tracking, L("Tracking"));
            tracking.CreateChildPermission(StrPermissions.Pages_Tracking_Create, L("CreateNewTracking"));
            tracking.CreateChildPermission(StrPermissions.Pages_Tracking_Edit, L("EditTracking"));
            tracking.CreateChildPermission(StrPermissions.Pages_Tracking_Delete, L("DeleteTracking"));

            var awb = pages.CreateChildPermission(StrPermissions.Pages_Awb, L("Awb"));
            awb.CreateChildPermission(StrPermissions.Pages_Awb_Create, L("CreateNewAwb"));
            awb.CreateChildPermission(StrPermissions.Pages_Awb_Edit, L("EditAwb"));
            awb.CreateChildPermission(StrPermissions.Pages_Awb_Delete, L("DeleteAwb"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, StrConsts.LocalizationSourceName);
        }

        private static ILocalizableString LEaf(string name)
        {
            return new LocalizableString(name, EafConsts.LocalizationSourceName);
        }
    }
}