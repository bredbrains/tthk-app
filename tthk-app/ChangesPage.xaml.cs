using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Fizzler;
using Microsoft.FSharp.Linq.RuntimeHelpers;
using Microsoft.Scripting.Utils;
using tthk_app.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Button = Xamarin.Forms.Button;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesPage
    {
        public ObservableCollection<ChangeGrouping<string, Change>> ChangeGroups { get; set; }

        private void LoadChanges()
        {
            var changes = ChangeCollection.GetChangeList();
            var groups = changes.GroupBy(c => c.Date).Select(g => new ChangeGrouping<string, Change>(g.Key, g));
            ChangeGroups = new ObservableCollection<ChangeGrouping<string, Change>>(groups);
            this.BindingContext = this;
        }

        private void ChecksConnection()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                LoadChanges();
                InitializeComponent();
            }
            else
            {
                Label noInternetText = new Label()
                {
                    Text = "Muudatused ei ole võimalik kuvada",
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    FontSize = 35,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
                Button checkAgainButton = new Button()
                {
                    Text = "Proovi uuesti"
                };
                StackLayout noInternetLayout = new StackLayout()
                {
                    Children = {noInternetText, checkAgainButton}
                };
                checkAgainButton.Clicked += CheckAgainButtonOnClicked;
                Content = noInternetLayout;
                DependencyService.Get<IMessage>().ShortSnackbar("Teil puudub ühendus.");
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
            ChecksConnection();
        }
    }
}