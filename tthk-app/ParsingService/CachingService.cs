using Xamarin.Essentials;

namespace tthk_app.ParsingService
{
    public class CachingService
    {
        public CachingService(string html)
        {
            Preferences.Set("html", html);
        }
    }
}