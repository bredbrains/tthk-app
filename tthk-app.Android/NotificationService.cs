using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(tthk_app.Droid.NotificationService))]
namespace tthk_app.Droid
{
    class NotificationService : Java.Lang.Object, NotoficationInterface
    {
        MainActivity act = new MainActivity();
        public void GetNotification(TimeSpan notificationTime)
        {
            act.SendMeAMessage(notificationTime);
        }

        public void CancelNotification()
        {
            act.CancelTheNotification();
        }
    }
}