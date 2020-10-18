using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            string name = Preferences.Get("name", "none");
            string group = Preferences.Get("group", "none");
            if (name != "none")
            {
                UserName.Text = name;
            }
            else
            {
                UserName.Text = null;
            }

            if (group != "none")
            {
                UserGroup.Text = group;
            }
            else
            {
                UserGroup.Text = null;
            }
        }

        private List<string> GenerateGroupEnds()
        {
            int nowYear = DateTime.Now.Year;
            int[] years = new int[] {nowYear, nowYear - 1, nowYear - 2, nowYear - 3};
            string[] yearsStrings = years.Select(year => year.ToString().Remove(0,2)).ToArray();
            string[] studyTypes = new string[] {"p", "g"};
            string[] langs = new string[] {"v", "e"};
            List<string> ends = new List<string>();
            foreach (string year in yearsStrings)
            {
                foreach (string studyType in studyTypes)
                {
                    foreach (string lang in langs)
                    {
                        ends.Add(studyType + lang + year);
                        ends.Add(studyType + lang + year + "-1");
                        ends.Add(studyType + lang + year + "-2");
                    }
                }
            }
            return ends;
        }

        private void AuthorsCellTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InfoPage());
        }

        private void UserName_OnCompleted(object sender, EventArgs e)
        {
            string value = UserName.Text;
            Preferences.Set("name", value);
        }

        private void UserGroup_OnCompleted(object sender, EventArgs e)
        {
            string value = UserGroup.Text;
            var groupEnds = GenerateGroupEnds();
            if (groupEnds.Any(x=> value.EndsWith(x)))
            {
                Preferences.Set("group", value);
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Rühm sisestatud valesti.");
            }
        }

        private void ChangesNotifcations_OnChanged(object sender, ToggledEventArgs e)
        {
            if (ChangesNotifcations.On)
            {
                DependencyService.Get<IChangesNotifications>().GetNotification(NotificationTimePicker.Time);
                DependencyService.Get<IMessage>().ShortAlert("Märguanded on lubatud - " + NotificationTimePicker.Time.ToString());
                DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + Math.Abs(DateTime.Now.Hour - NotificationTimePicker.Time.Hours).ToString() + " tunni ja " + Math.Abs(DateTime.Now.Minute - NotificationTimePicker.Time.Minutes).ToString() + " minuti pärast.");
                if (NotificationTimePicker.Time.Hours > 12 || DateTime.Now.Hour > 12)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + (24 - Math.Abs(DateTime.Now.Hour - NotificationTimePicker.Time.Hours)).ToString() + " tunni ja " + Math.Abs(DateTime.Now.Minute - NotificationTimePicker.Time.Minutes).ToString() + " minuti pärast.");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + Math.Abs(DateTime.Now.Hour - NotificationTimePicker.Time.Hours).ToString() + " tunni ja " + Math.Abs(DateTime.Now.Minute - NotificationTimePicker.Time.Minutes).ToString() + " minuti pärast.");
                }
            }
            else
            {
                DependencyService.Get<IChangesNotifications>().CancelNotification();
                DependencyService.Get<IMessage>().ShortAlert("Hoiatused on keelatud");
            }
        }
    }
}