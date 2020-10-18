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
            if (notificationTime.Hours > 12 || DateTime.Now.Hour > 12 || notificationTime.Hours == DateTime.Now.Hour) { hour = Math.Abs(notificationTime.Hours - DateTime.Now.Hour) * 3600000; }
            else { hour = (24 - Math.Abs(notificationTime.Hours - DateTime.Now.Hour)) * 3600000; }


            if (hour != 0)
            {
                if (notificationTime.Minutes != 0) { minute = (60 - Math.Abs(notificationTime.Minutes - DateTime.Now.Minute)) * 60000; hour = hour - 3600000; }
                else { minute = 0; }
            }
            else
            {
                minute = Math.Abs(notificationTime.Minutes - DateTime.Now.Minute) * 60000;
            }


            if (minute != 0)
            {
                if (notificationTime.Seconds != 0) { second = (60 - Math.Abs(notificationTime.Seconds - DateTime.Now.Second)) * 1000; minute = minute - 60000; }
                else { second = 0; }
            }
            else
            {
                second = Math.Abs(notificationTime.Seconds - DateTime.Now.Second) * 1000;
            }


            time = hour + minute + second;

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan tsEpoch = DateTime.UtcNow.Subtract(epoch);
            long milliSinceEpoch = (long)tsEpoch.TotalMilliseconds;

            alarmManager = (AlarmManager)Instance.GetSystemService(Context.AlarmService);
            alarmIntent = new Intent(Instance, typeof(BackgroundReceiver));
            alarmIntent.PutExtra("message", time.ToString());
            pending = PendingIntent.GetBroadcast(Instance, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            alarmManager.SetRepeating(AlarmType.RtcWakeup, milliSinceEpoch + time, AlarmManager.IntervalDay, pending);
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
    }
}