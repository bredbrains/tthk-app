using System;
using System.ComponentModel;
using Xamarin.Essentials;

namespace tthk_app
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]

    public partial class MainPage
    {
        [assembly: ExportFont("Samantha.ttf", Alias = "Samantha")]
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string name = Preferences.Get("name", "none");
            YourGroup.Text = "Teie grupp: " + name;
            base.OnAppearing();
        }

        private void InfoButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InfoPage());
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