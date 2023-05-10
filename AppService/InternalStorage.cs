using System.Collections.Concurrent;

namespace AppService
{
    internal static class InternalStorage
    {
        private static ConcurrentDictionary<Guid, object> _storage;

        static InternalStorage()
        {
            _storage = new ConcurrentDictionary<Guid, object>();
        }

        public static void AddProject(Guid projectHandle, object project)
        {
            if (_storage.ContainsKey(projectHandle))
            {
                object obj;
                _storage.TryRemove(projectHandle, out obj);
            }

            _storage.TryAdd(projectHandle, project);
        }

        public static object RetrieveProject(Guid projectHandle)
        {
            object output = null;
            if (_storage.TryGetValue(projectHandle, out output))
                return output;

            return output;
        }
    }
}
