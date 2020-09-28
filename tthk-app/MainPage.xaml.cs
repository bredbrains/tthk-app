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
            YourGroup.Text = "Teie grupp: " + GroupPicker.Items[GroupPicker.SelectedIndex];
        }
    }
}