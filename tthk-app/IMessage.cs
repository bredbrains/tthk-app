namespace tthk_app
{
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
        void LongSnackbar(string message);
        void ShortSnackbar(string message);
    }
}