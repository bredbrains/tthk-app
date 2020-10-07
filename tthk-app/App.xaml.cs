using System;
using System.IO;
using System.Reflection;
using tthk_app.Models;

namespace tthk_app
{
    public partial class App
    {
        static ChangeDatabase database;
        public static ChangeDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ChangeDatabase();
                }
                return database;
            }
        }
        
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        
    }
}