using System;
using System.Windows.Input;
using Xamarin.Forms.Xaml;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesPage
    {
        private ICommand refreshCommand;

        public ChangesPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            // Parse button.
        }
    }
}