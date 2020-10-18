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
        public bool Notifications;
        public Intent AlarmIntent;
        public PendingIntent Pending;
        public AlarmManager AlarmManager;
        public Calendar Time;
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
            Notifications = false;
        }

        public void GetTodayChangesForUser()
        {
            
        }

        public void SendMeAMessage(TimeSpan notificationTime)
        {
            long time = 0;
            if (notificationTime.Hours > 12)
            {
                time = 24 - Math.Abs(DateTime.Now.Hour - notificationTime.Hours) * 1000 * 60 * 60 + Math.Abs(DateTime.Now.Minute - notificationTime.Minutes) * 1000 * 60;
            }
            else if (notificationTime.Hours < 12)
            {
                time = Math.Abs(DateTime.Now.Hour - notificationTime.Hours) * 1000 * 60 * 60 + Math.Abs(DateTime.Now.Minute - notificationTime.Minutes) * 1000 * 60;
            }
            else
            {
                time = 0;
            }

            AlarmManager = (AlarmManager)Instance.GetSystemService(AlarmService);
            AlarmIntent = new Intent(Instance, typeof(BackgroundReceiver));
            AlarmIntent.PutExtra("message", DateTime.Now.ToString(CultureInfo.CurrentCulture));
            Pending = PendingIntent.GetBroadcast(Instance, 0, AlarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager?.SetRepeating(AlarmType.RtcWakeup, time, AlarmManager.IntervalDay, Pending);
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