using Android.App;
using Android.Support.Design.Widget;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(tthk_app.Droid.MessageAndroid))]

namespace tthk_app.Droid
{
    public class MessageAndroid : IMessage
    {
        private Android.Views.View View
        {
            get
            {
                var activity = (Activity) Forms.Context;
                var view = activity.FindViewById(Android.Resource.Id.Content);
                return view;
            }
        }

        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }

        public void ShortSnackbar(string message)
        {
            Snackbar.Make(View, message, Snackbar.LengthLong).Show();
        }

        public void LongSnackbar(string message)
        {
            Snackbar.Make(View, message, Snackbar.LengthLong).Show();
        }
    }
}