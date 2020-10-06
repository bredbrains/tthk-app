using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using tthk_app.Models;
using tthk_app.ParsingService;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesPage : ContentPage
    {
        public ObservableCollection<ChangeGrouping<string, Change>> ChangeGroups { get; set; }

        private void LoadChanges(IEnumerable<Change> _changes)
        {
            var changes = _changes;
            var groups = changes.GroupBy(c => c.Date).Select(g => new ChangeGrouping<string, Change>(g.Key, g));
            ChangeGroups = new ObservableCollection<ChangeGrouping<string, Change>>(groups);
            this.BindingContext = this;
        }

        private async Task ChecksConnection()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                Title = "Saan muudatused...";
                ActivityIndicator activityIndicator = new ActivityIndicator
                {
                    IsRunning = true,
                    Margin = 175,
                    Color = Color.FromHex("#A22538")
                };

                if (ChangesListView == null)
                {
                    if (ChangeCollection.GetChangeList(false) != null)
                    {
                        await Task.Run(() => { LoadChanges(ChangeCollection.GetChangeList(false)); });

                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;
                        InitializeComponent();

                        Title = "Tunniplaani muudatused";
                        Content = ChangesListView;
                        ChangesListView.IsRefreshing = false;
                    }
                    else
                    {
                        StackLayout stack = new StackLayout();

                        Label text = new Label();
                        text.Text = "Muudatused puuduvad";

                        Button btn = new Button();
                        btn.Text = "Proovi";

                        stack.Children.Add(text);
                        stack.Children.Add(btn);
                        Content = stack;
                    }
                }
                else
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    DependencyService.Get<IMessage>().ShortAlert("Tunniplaani muudatused puuduvad.");
                }

                ChangesListView.RefreshControlColor = Color.FromHex("#A22538");
            }
            else
            {
                ActivityIndicator activityIndicator = new ActivityIndicator
                {
                    IsRunning = true,
                    Margin = 175,
                    Color = Color.FromHex("#A22538")
                };
                if (Preferences.ContainsKey("html"))
                {
                    if (ChangeCollection.GetChangeList(true) != null)
                    {
                        await Task.Run(() => { LoadChanges(ChangeCollection.GetChangeList(true)); });

                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;
                        InitializeComponent();

                        Title = "Tunniplaani muudatused";
                        Content = ChangesListView;
                        ChangesListView.IsRefreshing = false;
                    }
                    else
                    {
                        StackLayout stack = new StackLayout();

                        Label text = new Label();
                        text.Text = "Muudatused puuduvad";

                        Button btn = new Button();
                        btn.Text = "Proovi";

                        stack.Children.Add(text);
                        stack.Children.Add(btn);
                        Content = stack;
                    }
                }
                else
                {
                    StackLayout stack = new StackLayout();

                    Label text = new Label();
                    text.Text = "Muudatused pole võimalik saada.";

                    Button btn = new Button();
                    btn.Text = "Proovi";

                    stack.Children.Add(text);
                    stack.Children.Add(btn);
                    Content = stack;
                }

                DependencyService.Get<IMessage>().ShortAlert("Teil puudub ühendus.");
            }
        }

        private void CheckAgainButtonOnClicked(object sender, EventArgs e)
        {
            ChecksConnection();
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
    }
}