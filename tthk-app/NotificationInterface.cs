using System;
using System.Collections.Generic;
using System.Text;

namespace tthk_app
{
    public interface NotoficationInterface
    {
        void GetNotification(TimeSpan notificationTime);
        void CancelNotification();
    }
}
