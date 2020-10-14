using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Xamarin.Forms;
using Plugin.NFC;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;
using Android.Content;
using Android.Icu.Util;
using Android.Nfc;
using Android.Widget;
using Xamarin.Essentials;
using static Java.Util.CalendarField;

namespace tthk_app.Droid
{
    [Activity(Label = "THK", Icon = "@mipmap/tthklogoapp", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "application/com.bredbrains.tthk_app")]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);

            // Plugin NFC: Initialization
            CrossNFC.Init(this);    

            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            bool notifications = Preferences.Get("changesNotifications", false);
            if (notifications)
            {
                DateTime notificationsTime = Preferences.Get("changesNotificationsTime", DateTime.Today);
                Calendar time = new GregorianCalendar();
                time.Set(CalendarField.HourOfDay, notificationsTime.Hour);
                time.Set(CalendarField.Minute, notificationsTime.Minute);
                time.Set(CalendarField.Second, notificationsTime.Minute);
                
                Calendar pendingTime = new GregorianCalendar();
                pendingTime.Set(CalendarField.HourOfDay, 24);
                var alarmIntent = new Intent(this, typeof(BackgroundReceiver));
                
                string userGroup = Preferences.Get("group", "none");
                
                alarmIntent.PutExtra("title", "Teie rühma muudatused.");
                alarmIntent.PutExtra("message", "Tere! Täna teie rühmal on järgmised muudatused:");
                var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
                var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
                alarmManager.SetRepeating(AlarmType.RtcWakeup, time.TimeInMillis,pendingTime.TimeInMillis,pending);
            }
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