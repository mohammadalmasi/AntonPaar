namespace AppService
{
    public interface IReporter
    {
        void LogStatus(string result);
        void LogProgress(decimal percentage);
    }
}
