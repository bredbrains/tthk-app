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
            notifications = false;
        }

        public void SendMeAMessage(TimeSpan notificationTime)
        { 
            if (notificationTime.Hours > 12 || DateTime.Now.Hour > 12)
            {
                time = ((24 - Math.Abs(DateTime.Now.Hour - notificationTime.Hours)) * 1000 * 60 * 60) + (Math.Abs(DateTime.Now.Minute - notificationTime.Minutes) * 1000 * 60);
            }
            else if (notificationTime.Hours < 12 || DateTime.Now.Hour < 12)
            {
                time = (Math.Abs(DateTime.Now.Hour - notificationTime.Hours) * 1000 * 60 * 60) + (Math.Abs(DateTime.Now.Minute - notificationTime.Minutes) * 1000 * 60);
            }
            else
            {
                time = 0;
            }

            alarmManager = (AlarmManager)Instance.GetSystemService(AlarmService);
            alarmIntent = new Intent(Instance, typeof(BackgroundReceiver));
            alarmIntent.PutExtra("message", DateTime.Now.ToString());
            pending = PendingIntent.GetBroadcast(Instance, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            alarmManager.SetRepeating(AlarmType.RtcWakeup, time, AlarmManager.IntervalFifteenMinutes, pending);
            //alarmManager.Set(AlarmType.RtcWakeup, 0, pending);
        }

        public void CancelTheNotification()
        {
            AlarmManager.Cancel(Pending);
            Pending.Cancel();
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