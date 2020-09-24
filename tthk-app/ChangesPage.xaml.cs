using System;
using tthk_app.ParsingService;
using Xamarin.Forms.Xaml;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesPage
    {
        public ChangesPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Parsed.Text = "";
            foreach (var x in ParserEngine.GetChanges())
            {
                foreach (var j in x)
                {
                    Parsed.Text += j + " ";
                }

                Parsed.Text += "<br>";
            }
        }
    }
}