using System;

namespace tthk_app
{
    public interface IChangesNotifications
    {
        void GetNotification(TimeSpan notificationTime);
        void CancelNotification();
    }
}
