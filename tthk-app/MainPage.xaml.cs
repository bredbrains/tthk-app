using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace tthk_app
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]

    public partial class MainPage
    {
        INotificationManager notificationManager;
        private int notificationNumber = 0;
        string[] estMonths = new string[] {"jaanuar", "veebruar", "märts", "aprill", "mai", "juuni", "juuli", "august", "september", "oktoober", "november", "detsember"};
        public MainPage()
        {
            InitializeComponent();
            notificationManager = DependencyService.Get<INotificationManager>();
            // GetNotification();
            /* notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            }; */
        }

        void GetNotification()
        {
            notificationNumber++;
            string title = $"Tunniplaani muudatused";
            string message = $"Teil puuduvad muudatused.";
            notificationManager.ScheduleNotification(title, message);
        }

        protected override void OnAppearing()
        {
            int TodayMonthNumber = DateTime.Now.Month;
            int TodayDayNumber = DateTime.Now.Day;
            TodayDateLabel.Text = $"{TodayDayNumber}. {estMonths[TodayMonthNumber-1]}";
            string name = Preferences.Get("name", "none");
            if (name != "none")
            {
                var nowHour = DateTime.Now.Hour;
                if (nowHour >= 4 && nowHour < 12)
                {
                    HelloToUser.Text = $"Tere hommikust, {name}!";
                }
                else if (nowHour >= 12 && nowHour < 16)
                {
                    HelloToUser.Text = $"Tere päevast, {name}!";
                }
                else if (nowHour >= 16 && nowHour < 23)
                {
                    HelloToUser.Text = $"Tere õhtust, {name}!";
                }
                else if (nowHour == 23 || nowHour < 4)
                {
                    HelloToUser.Text = $"Head ööd, {name}!";
                }
            }
            else
            {
                HelloToUser.Text = "Tere!";
            }
            base.OnAppearing();
        }

        private void SettingsButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        private async void OpenTelegramChannel()
        {
            await Browser.OpenAsync("https://t.me/toostusharidus");
        }

        private void TelegramButtonClick(object sender, EventArgs e)
        {
            OpenTelegramChannel();
        }
    }
}