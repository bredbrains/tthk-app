﻿using Shiny;
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
            string pickerTime = Preferences.Get("pickerTime", "none");
            string switchValue = Preferences.Get("switchValue", "none");

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

            if (pickerTime != "none")
            {
                NotificationTimePicker.Time = TimeSpan.Parse(pickerTime);
            }
            else
            {
                NotificationTimePicker.Time = TimeSpan.Parse("8:00:00");
            }

            if (switchValue != "none")
            {
                ChangesNotifcations.On = true;
            }
            else
            {
                ChangesNotifcations.On = false;
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
                Preferences.Set("switchValue", "on");
                DependencyService.Get<IChangesNotifications>().GetNotification(NotificationTimePicker.Time);


                double exactTime;
                TimeSpan time;

                if (DateTime.Now.TimeOfDay.TotalMilliseconds > (12 * 3600000) || NotificationTimePicker.Time.TotalMilliseconds > (12 * 3600000))
                {
                    if (DateTime.Now.TimeOfDay.TotalMilliseconds > (12 * 3600000) || DateTime.Now.TimeOfDay.TotalMilliseconds > NotificationTimePicker.Time.TotalMilliseconds)
                    {
                        exactTime = (24 * 3600000) - DateTime.Now.TimeOfDay.Subtract(NotificationTimePicker.Time).TotalMilliseconds;
                    }
                    else
                    {
                        exactTime = (24 * 3600000) - NotificationTimePicker.Time.Subtract(DateTime.Now.TimeOfDay).TotalMilliseconds;
                    }
                }
                else 
                {
                    if (DateTime.Now.TimeOfDay.TotalMilliseconds > NotificationTimePicker.Time.TotalMilliseconds)
                    {
                        exactTime = DateTime.Now.TimeOfDay.Subtract(NotificationTimePicker.Time).TotalMilliseconds;
                    }
                    else
                    {
                        exactTime = NotificationTimePicker.Time.Subtract(DateTime.Now.TimeOfDay).TotalMilliseconds;
                    }
                }

                time = TimeSpan.FromMilliseconds(exactTime);

                DependencyService.Get<IMessage>().ShortAlert("Esimene teade " + time.Hours.ToString() + " tunni ja " + time.Minutes.ToString() + " minuti pärast.");
            }
            else
            {
                Preferences.Set("switchValue", "none");
                DependencyService.Get<IChangesNotifications>().CancelNotification();
                DependencyService.Get<IMessage>().ShortAlert("Hoiatused on keelatud");
            }
        }

        private void NotificationTimePicker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                Preferences.Set("pickerTime", NotificationTimePicker.Time.ToString());
            }
        }
    }
}