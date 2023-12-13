using Eaf;
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