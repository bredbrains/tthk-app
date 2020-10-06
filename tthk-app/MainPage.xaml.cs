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
        string[] estMonths = new string[] {"jaanuar", "veebruar", "märts", "aprill", "mai", "juuni", "juuli", "august", "september", "oktoober", "november", "detsember"};
        public MainPage()
        {
            InitializeComponent();
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

        private void InfoButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }
        
        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string value = GroupPicker.Items[GroupPicker.SelectedIndex];
            Preferences.Set("name", value);
            YourGroup.Text = "Teie rühm: " + GroupPicker.Items[GroupPicker.SelectedIndex];
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