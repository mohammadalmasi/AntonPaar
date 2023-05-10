using System.Collections;

namespace AppService
{
    internal interface IFileReader
    {
        public void Read();
        public void StopRead();
        public void AddDetectedObject(Hashtable detectedObjects, string detectedObject);
        public decimal GetProgress(long fileBytesLength, long position);
    }
}
