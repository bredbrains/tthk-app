using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(tthk_app.Droid.AndroidNotificationManager))]

namespace tthk_app.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string ChannelId = "default";
        const string ChannelName = "Default";
        const string ChannelDescription = "The default channel for notifications.";
        const int PendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool _channelInitialized;
        int _messageId = -1;
        NotificationManager _manager;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            CreateNotificationChannel();
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!_channelInitialized)
            {
                CreateNotificationChannel();
            }

            _messageId++;

            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, PendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.tthklogo))
                .SetSmallIcon(Resource.Drawable.tthklogo)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            _manager.Notify(_messageId, notification);

            return _messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        void CreateNotificationChannel()
        {
            _manager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(ChannelName);
                var channel = new NotificationChannel(ChannelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = ChannelDescription
                };
                _manager.CreateNotificationChannel(channel);
            }

            _channelInitialized = true;
        }
    }
}