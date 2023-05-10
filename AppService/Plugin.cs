namespace AppService
{
    public class Plugin : IPlugin
    {
        public Plugin()
        {

        }

        public void Start(Guid projectHandle, string path, IReporter reporter)
        {
            var fileReader = new FileReader(path, reporter);
            InternalStorage.AddProject(projectHandle, fileReader);
            fileReader.Read();
        }

        public void Stop(Guid projectHandle)
        {
            var fileReader = InternalStorage.RetrieveProject(projectHandle) as FileReader;
            fileReader?.StopRead();
        }
    }
}
