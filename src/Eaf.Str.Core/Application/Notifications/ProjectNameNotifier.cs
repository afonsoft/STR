using System.Threading.Tasks;
using Eaf;
using Eaf.Domain.Services;
using Eaf.Localization;
using Eaf.Middleware.Authorization.Users;
using Eaf.Notifications;

namespace Eaf.Str.Notifications
{
    public class ProjectNameNotifier : DomainService, IProjectNameNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public ProjectNameNotifier(
            INotificationPublisher notificationPublisher
        )
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                ProjectNameNotificationNames.SimpleMessage,
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { user }
                );
        }
    }
}