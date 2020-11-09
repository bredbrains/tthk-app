using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using tthk_app.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesPage : ContentPage
    {
        private ActivityIndicator activityIndicator;
        private async Task GetChangesFromDatabase()
        {
            var databaseItems = await App.Database.GetItemsAsync();
            if (databaseItems.Count > 0)
            {
                LoadChanges(databaseItems);
            }
            else
            {
                DependencyService.Get<IMessage>().ShortSnackbar("Teil puuduvad salvestatud andmed.");
            }
        }
        char[] estDayOfWeeks = new char[8] {'E', 'T', 'K', 'N', 'R', 'L', 'P', '?'};
        public ObservableCollection<ChangeGrouping<string, Change>> ChangeGroups { get; set; }
        public List<Change> ParsedChanges { get; set; }
        private void LoadChanges(IEnumerable<Change> _changes)
        {
            ParsedChanges = _changes as List<Change>;
            var changes = _changes;
            var groups = changes.OrderBy(c => c.Lesson).GroupBy(c => estDayOfWeeks[c.DayOfWeek-1] + ", " + c.Date).Select(g => new ChangeGrouping<string, Change>(g.Key, g));
            ChangeGroups = new ObservableCollection<ChangeGrouping<string, Change>>(groups);
            this.BindingContext = this;
        }
        private async Task ChecksConnection()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                Title = "Saan muudatused...";
                activityIndicator = new ActivityIndicator
                {
                    IsRunning = true, Margin = 175, Color = Color.FromHex("#A22538")
                };
                if (ChangesListView == null)
                {
                    Content = activityIndicator;
                }
                await Task.Run(() => { LoadChanges(ChangeCollection.GetChangeList()); });
                if (ChangesListView == null)
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    InitializeComponent();
                    string group = Preferences.Get("group", "none");
                    if (group != "none")
                    {
                        ChangesPageSearchBar.Placeholder = group;
                    }
                }
                else
                {
                    Title = "Tunniplaani muudatused";
                    string group = Preferences.Get("group", "none");
                    if (group != "none")
                    {
                        ChangesPageSearchBar.Placeholder = group;
                        Content = ChangesListView;
                    }
                }
                ChangesListView.IsRefreshing = false;
                ChangesListView.RefreshControlColor = Color.FromHex("#A22538");
            }
            else
            {
                Title = "Saan muudatused...";
                activityIndicator = new ActivityIndicator
                {
                    IsRunning = true, Margin = 175, Color = Color.FromHex("#A22538")
                };
                if (ChangesListView == null)
                {
                    Content = activityIndicator;
                }

                await GetChangesFromDatabase();
                if (ChangesListView == null)
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsEnabled = false;
                    InitializeComponent();
                    ChangesListView.IsRefreshing = false;
                    string group = Preferences.Get("group", "none");
                    if (group != "none")
                    {
                        ChangesPageSearchBar.Placeholder = group;
                    }
                }
                else
                {
                    ChangesListView.IsRefreshing = false;
                    Content = ChangesListView;
                }

                DependencyService.Get<IMessage>().ShortAlert("Teil puudub ühendus.");
            }
        }
        public ChangesPage()
        {
            ChecksConnection();
        }
        private void ChangesListView_OnRefreshing(object sender, EventArgs e)
        {
            ChangesListView.IsRefreshing = true;
            ChecksConnection();
        }

        private void ChangesListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var content = e.Item as Change;
                if (content != null)
                {
                    string text;
                    text = $"Tunniplaani muudatus: \n{estDayOfWeeks[content.DayOfWeek-1]}, {content.Date}\nTunnid: {content.Lesson}\n{content.Group}\n{content.Teacher}\n{content.Room}";
                    ShareText(text);
                }
            }
            ((ListView)sender).SelectedItem = null;
        }
        public async void ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Saata muudatus"
            });
        }
        private void ChangesPageSearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {   
            SearchBar searchBar = sender as SearchBar;
            if (searchBar != null && searchBar.Text != null && ParsedChanges != null)
            {
                List<Change> searchedChanges = new List<Change>();
                string text = searchBar.Text;
                foreach (var change in ParsedChanges)
                {
                    if (change.Group.Contains(text) || change.Teacher.Contains(text) || change.Room.Contains(text))
                    {
                        searchedChanges.Add(change);
                    }
                }
                var groups = searchedChanges.GroupBy(c => estDayOfWeeks[c.DayOfWeek-1] + ", " + c.Date).Select(g => new ChangeGrouping<string, Change>(g.Key, g));
                var collectionOfChangesGroups = new ObservableCollection<ChangeGrouping<string, Change>>(groups);
                ChangesListView.ItemsSource = collectionOfChangesGroups;
                Content = ChangesListView;
                if (searchedChanges.Count == 0)
                {
                    Content = new Label()
                    {
                        Text = "Kahjuks, midagi pole leitud.",
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    };
                }
            }
        }
    }
}