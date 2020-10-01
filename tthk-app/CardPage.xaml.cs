using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.NFC;

namespace tthk_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPage
    {
        public CardPage()
        {
            InitializeComponent();
            CrossNFC.Current.StartListening();
            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
        }

        private async void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            lbl.Text = tagInfo.SerialNumber + "\n";
            foreach(var x in tagInfo.Records)
            {
                lbl.Text += x.ExternalType;
                lbl.Text += x.Message;
            }
            CrossNFC.Current.StopListening();
            CrossNFC.Current.StartPublishing();
            CrossNFC.Current.PublishMessage(tagInfo);
        }
    }
}