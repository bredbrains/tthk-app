using System;
using System.Globalization;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Xamarin.Forms;
using Plugin.NFC;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;
using Android.Content;
using Android.Nfc;
using Calendar = Android.Icu.Util.Calendar;
using Android.Icu.Util;

namespace tthk_app.Droid
{
    [Activity(Label = "THK", Icon = "@mipmap/tthklogoapp", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "application/com.bredbrains.tthk_app")]

    public class MainActivity : FormsAppCompatActivity
    {
        public Intent alarmIntent;
        public PendingIntent pending;
        public AlarmManager alarmManager;
        long time;
        long hour;
        long minute;
        long second;
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Platform.Init(this, bundle);
            Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public void SendMeAMessage(TimeSpan notificationTime)
        {
            double exactTime;
            TimeSpan time;

            if (DateTime.Now.TimeOfDay.TotalMilliseconds > (12 * 3600000) || notificationTime.TotalMilliseconds > (12 * 3600000))
            {
                if (DateTime.Now.TimeOfDay.TotalMilliseconds > (12 * 3600000) && DateTime.Now.TimeOfDay.TotalMilliseconds > notificationTime.TotalMilliseconds || DateTime.Now.TimeOfDay.TotalMilliseconds > (12 * 3600000))
                {
                    exactTime = (24 * 3600000) - DateTime.Now.TimeOfDay.Subtract(notificationTime).TotalMilliseconds;
                }
                else
                {
                    exactTime = (24 * 3600000) - notificationTime.Subtract(DateTime.Now.TimeOfDay).TotalMilliseconds;
                }
            }
            else
            {
                if (DateTime.Now.TimeOfDay.TotalMilliseconds > notificationTime.TotalMilliseconds)
                {
                    exactTime = DateTime.Now.TimeOfDay.Subtract(notificationTime).TotalMilliseconds;
                }
                else
                {
                    exactTime = notificationTime.Subtract(DateTime.Now.TimeOfDay).TotalMilliseconds;
                }
            }

            if (exactTime > 24 * 3600000){ time = TimeSpan.FromMilliseconds(exactTime - (24 * 3600000)); }
            else { time = TimeSpan.FromMilliseconds(exactTime); }

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan tsEpoch = DateTime.UtcNow.Subtract(epoch);
            long milliSinceEpoch = (long)tsEpoch.TotalMilliseconds;

            alarmManager = (AlarmManager)Instance.GetSystemService(Context.AlarmService);
            alarmIntent = new Intent(Instance, typeof(BackgroundReceiver));
            alarmIntent.PutExtra("message", time.ToString());
            pending = PendingIntent.GetBroadcast(Instance, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            alarmManager.SetRepeating(AlarmType.RtcWakeup, milliSinceEpoch + Convert.ToInt64(time.TotalMilliseconds), AlarmManager.IntervalFifteenMinutes, pending);
            //Instance.alarmManager.Set(AlarmType.RtcWakeup, 0, pending);
        }

        public void CancelTheNotification()
        {
            alarmManager.Cancel(pending);
            pending.Cancel();
        }
        protected override void OnNewIntent(Intent intent)
        {
            // base.OnNewIntent(intent);
            // CreateNotificationFromIntent(intent);

            // Plugin NFC: Tag Discovery Interception
            CrossNFC.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Intent broadcastIntent = new Intent(Instance, typeof(BackgroundReceiver));
            SendBroadcast(broadcastIntent);
        }
    }
}