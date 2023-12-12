using System;
using System.Threading.Tasks;
using Eaf;
using Eaf.Middleware.Authorization.Users;
using Eaf.Notifications;

namespace Eaf.Str.Notifications
{
    public interface IProjectNameNotifier
    {
        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}