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
                long hour;
                long minute;
                long second;
                DependencyService.Get<IChangesNotifications>().GetNotification(NotificationTimePicker.Time);


                if (NotificationTimePicker.Time.Hours > 12 || DateTime.Now.Hour > 12 || NotificationTimePicker.Time.Hours == DateTime.Now.Hour) { hour = Math.Abs(NotificationTimePicker.Time.Hours - DateTime.Now.Hour); }
                else { hour = (24 - Math.Abs(NotificationTimePicker.Time.Hours - DateTime.Now.Hour)); }


                if (hour != 0)
                {
                    if (NotificationTimePicker.Time.Minutes != 0) { minute = (60 - Math.Abs(NotificationTimePicker.Time.Minutes - DateTime.Now.Minute)); hour = hour - 1; }
                    else { minute = 0; }
                }
                else
                {
                    minute = Math.Abs(NotificationTimePicker.Time.Minutes - DateTime.Now.Minute);
                }


                if (minute != 0)
                {
                    if (NotificationTimePicker.Time.Seconds != 0) { second = (60 - Math.Abs(NotificationTimePicker.Time.Seconds - DateTime.Now.Second)); minute = minute - 1; }
                    else { second = 0; }
                }
                else
                {
                    second = Math.Abs(NotificationTimePicker.Time.Seconds - DateTime.Now.Second);
                }

                if (hour != 0 && minute == 0)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + hour.ToString() + " minuti pärast.");
                }
                else if (hour != 0 && minute != 0)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + hour.ToString() + " tunni ja " + minute.ToString() + " minuti pärast.");
                }
                else if (minute != 0 && hour == 0)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + minute.ToString() + " minuti pärast.");
                }
                else if (minute != 0 && second != 0)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + minute.ToString() + " minuti pärast. " + minute.ToString() + " sekundi pärast.");
                }
                else if (second != 0 && minute == 0)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + second.ToString() + " sekundi pärast.");
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