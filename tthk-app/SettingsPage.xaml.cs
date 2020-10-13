﻿using System;
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
            bool notifications = Preferences.Get("changesNotifications", false);
            DateTime notificationsTime = Preferences.Get("changesNotificationsTime", DateTime.Today);
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
            
            if (notifications)
            {
                ChangesNotifcations.On = true;
            }

            if (notificationsTime != DateTime.Today && ChangesNotifcations.On)
            {
                NotificationTimePicker.IsEnabled = true;
                NotificationTimePicker.Time = new TimeSpan(notificationsTime.Hour, notificationsTime.Minute, notificationsTime.Second);
            }
            else
            {
                NotificationTimePicker.IsEnabled = false;
                NotificationTimePicker.Time = new TimeSpan(notificationsTime.Hour, notificationsTime.Minute, notificationsTime.Second);
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
                Preferences.Set("changesNotifications", true);
                NotificationTimePicker.IsEnabled = true;
            }
            else
            {
                Preferences.Set("changesNotifications", false);
                NotificationTimePicker.IsEnabled = false;
            }
        }

        private void NotificationTimeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time" && ChangesNotifcations.On)
            {
                TimePicker tp = sender as TimePicker;
                TimeSpan pickedTime = tp.Time;
                DateTime changesTime = new DateTime(1, 1, 1) + pickedTime;
                Preferences.Set("changesNotificationsTime", changesTime);
            }
        }
    }
}