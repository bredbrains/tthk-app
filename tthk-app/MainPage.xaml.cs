using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using tthk_app.Models;
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

        string[] estMonths = new string[]
        {
            "jaanuar", "veebruar", "märts", "aprill", "mai", "juuni", "juuli", "august", "september", "oktoober",
            "november", "detsember"
        };

        char[] estDayOfWeeks = new char[7] {'E', 'T', 'K', 'N', 'R', 'L', 'P'};

        private void AddChangeToLabels(Change change, string userGroup)
        {
            DateTime changeDateTime = DateTime.ParseExact(change.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            if (changeDateTime == DateTime.Now.Date && change.Group.Contains(userGroup))
            {
                TodayChangesInLabel.Text += $"Tund: {change.Lesson} Õpetaja: {change.Teacher} Ruum: {change.Room}<br>";
                TodayChangesInLabel.TextType = TextType.Html;
                TodayChangesInLabel.Text = TodayChangesInLabel.Text.TrimEnd();
            }
            else if (changeDateTime > DateTime.Now.Date && change.Group.Contains(userGroup))
            {
                LaterChangesInLabel.Text +=
                    $"<b>{estDayOfWeeks[change.DayOfWeek - 1].ToString()}, {change.Date}<br>Tund: {change.Lesson} Õpetaja: {change.Teacher} Ruum: {change.Room}<br>";
                LaterChangesInLabel.TextType = TextType.Html;
                LaterChangesInLabel.Text = LaterChangesInLabel.Text.TrimEnd();
            }
        }

        private void SetNoneChangesToLabel(Label label)
        {
            label.Text = "Muudatused puuduvad";
        }

        private void CheckLabelsForBlankValues()
        {
            if (TodayChangesInLabel.Text.Length == 0)
            {
                SetNoneChangesToLabel(TodayChangesInLabel);
            }
            if (LaterChangesInLabel.Text.Length == 0)
            {
                SetNoneChangesToLabel(LaterChangesInLabel);
            }
        }

        private async void SortChangesByDates()
        {
            string userGroup = Preferences.Get("group", "none");
            var currentNetwork = Connectivity.NetworkAccess;

            if (currentNetwork == NetworkAccess.Internet && userGroup != "none")
            {
                var changes = GetChangesFromInternet() as List<Change>;
                if (changes != null && changes.Count > 0)
                {
                    TodayChangesInLabel.Text = "";
                    LaterChangesInLabel.Text = "";
                    foreach (var change in changes)
                    {
                        AddChangeToLabels(change, userGroup);
                    }
                    CheckLabelsForBlankValues();
                }
                else
                {
                    SetNoneChangesToLabel(TodayChangesInLabel);
                    SetNoneChangesToLabel(LaterChangesInLabel);
                }
            }
            else
            {
                var changes = await GetChangesFromDatabase();
                if (changes != null && changes.Count > 0)
                {
                    TodayChangesInLabel.Text = "";
                    LaterChangesInLabel.Text = "";
                    foreach (var change in changes)
                    {
                        AddChangeToLabels(change, userGroup);
                    }
                    CheckLabelsForBlankValues();
                }
                else
                {
                    SetNoneChangesToLabel(TodayChangesInLabel);
                    SetNoneChangesToLabel(LaterChangesInLabel);
                }
            }

            if (userGroup == "none")
            {
                FastChangesLayout.Children.Clear();
                FastChangesLayout.Children.Add(new Label()
                {
                    Text = "Teil pole ei ole määratud rühm.",
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold
                });
            }

            MainPageRefreshView.IsRefreshing = false;
        }
        
        private async Task<List<Change>> GetChangesFromDatabase()
        {
            return await App.Database.GetItemsAsync();
        }

        private IEnumerable<Change> GetChangesFromInternet()
        {
            return ChangeCollection.GetChangeList();
        }
        
        public MainPage()
        {
            InitializeComponent();
            MainPageRefreshView.IsRefreshing = true;
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
            SortChangesByDates();
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