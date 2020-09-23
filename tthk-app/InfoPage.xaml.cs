using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage
    {
        public InfoPage()
        {
            InitializeComponent();
            Shell.SetTabBarIsVisible(this, false);
        }
    }
}