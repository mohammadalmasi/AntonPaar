namespace AppService
{
    public interface IPlugin
    {
        void Start(Guid projectHandle, string path, IReporter reporter);

        void Stop(Guid projectHandle);
    }
}
