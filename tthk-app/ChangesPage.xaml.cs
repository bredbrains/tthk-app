using tthk_app.Models;
using Xamarin.Forms.Xaml;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesPage
    {
        public ChangesPage()
        {
            InitializeComponent();
            Change change = new Change();
        }
    }
}