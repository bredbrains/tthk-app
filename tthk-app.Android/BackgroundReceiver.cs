using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;

namespace tthk_app.Droid
{
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        public const string URGENT_CHANNEL = "com.bredbrains.tthkapp";
        public const int NOTIFY_ID = 1100;
        public const string NOTIFICATION_CHANNEL_ID = "tthkMuudatused";
        public override void OnReceive(Android.Content.Context context, Intent intent)
        {
            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");

            var importance = NotificationImportance.High;
            NotificationChannel chan = new NotificationChannel(URGENT_CHANNEL, "Urgent", importance);
            chan.EnableVibration(true);
            chan.LockscreenVisibility = NotificationVisibility.Public;
            
            var resultIntent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.tthklogoapp)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetContentIntent(pendingIntent)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true)
                .SetChannelId(URGENT_CHANNEL);

            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(chan);

            notificationManager.Notify(NOTIFY_ID, notificationBuilder.Build());
        }
    }
}