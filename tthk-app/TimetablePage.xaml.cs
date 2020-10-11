using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System;
using System.Net;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimetablePage : ContentPage
    {

        public TimetablePage()
        {
            InitializeComponent();
            TimeTable.Source = "https://thk.edupage.org/timetable/";
            TimeTable.Cookies = new CookieContainer();
        }
    }
}