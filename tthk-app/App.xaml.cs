using Xamarin.Forms;
using Plugin.NFC;

[assembly: ExportFont("SFProFont.ttf", Alias = "SFProFont")]
namespace tthk_app
{
    public partial class App
    {
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