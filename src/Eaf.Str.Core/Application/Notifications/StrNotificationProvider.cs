using Eaf.Authorization;
using Eaf.Localization;
using Eaf.Middleware.Authorization;
using Eaf.Notifications;

namespace Eaf.Str.Notifications
{
    public class StrNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    StrNotificationNames.SimpleMessage,
                    displayName: L("TestSendMensage"),
                    permissionDependency: new SimplePermissionDependency(MiddlewarePermissions.Pages_Administration_Users)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, StrConsts.LocalizationSourceName);
        }
    }
}