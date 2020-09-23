using System;
using System.Collections.Generic;
using System.Windows.Input;
using tthk_app.ParsingService;
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
            Parsed.Text = "";
            foreach (List<string> x in Parser.GetChanges())
            {
                foreach (string j in x)
                {
                    Parsed.Text += j + " ";
                }
            }

            Parsed.Text += "<br>";
        }
    }
}