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
            GetNotification();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            };
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
                HelloToUser.Text = $"Tere, {name}!";
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