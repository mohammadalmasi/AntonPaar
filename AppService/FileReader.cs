using System.Collections;
using System.Text;

namespace AppService
{
    internal class FileReader : FileStream, IFileReader
    {
        private readonly IReporter _reporter;
        private CancellationTokenSource _cancellationTokenSource;

        public FileReader(string path, IReporter reporter) : base(path, FileMode.Open)
        {
            _reporter = reporter;
        }

        public void Read()
        {
            List<byte> ByteArray = new List<byte>();
            Hashtable DetectedObjects = new Hashtable();
            _cancellationTokenSource = new CancellationTokenSource();

            _reporter.LogStatus("Starting MLA Application ...");

            while (Position < Length)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    _reporter.LogStatus("Application is stop by user ...");
                    Close();
                    break;
                }

                byte code = (byte)ReadByte();
                ByteArray.Add(code);

                if (code == 32)//code=32 means space
                {
                    ByteArray.Remove(ByteArray.Last());//remove space at the end of each word
                    string detectedObject = Encoding.UTF8.GetString(ByteArray.ToArray());
                    AddDetectedObject(DetectedObjects, detectedObject);
                    _reporter.LogStatus(string.Concat(Position, " : ", detectedObject, "---repeated : ", (int)DetectedObjects[detectedObject]));
                    ByteArray.Clear();
                }
                if (code == 10)//code=32 means enter
                {
                    ByteArray.Remove(ByteArray.Last());//remove enter(\r\n) at the end of each word
                    ByteArray.Remove(ByteArray.Last());//because we have to remove two byte
                    string detectedObject = Encoding.UTF8.GetString(ByteArray.ToArray());
                    _reporter.LogStatus(string.Concat(Position, " : ", detectedObject, "---repeated : ", (int)DetectedObjects[detectedObject]));
                    AddDetectedObject(DetectedObjects, detectedObject);
                    ByteArray.Clear();
                }
                if (Position == Length)
                {
                    string detectedObject = Encoding.UTF8.GetString(ByteArray.ToArray());
                    AddDetectedObject(DetectedObjects, detectedObject);
                    _reporter.LogStatus(string.Concat(Position, " : ", detectedObject, "---repeated : ", (int)DetectedObjects[detectedObject]));
                }

                _reporter.LogProgress(GetProgress(Length, Position));
            }

            Close();

            //Adding and reporting all of detected objects
            foreach (DictionaryEntry detectedObject in DetectedObjects)
            {
                _reporter.LogStatus(string.Concat(detectedObject.Key, "---", detectedObject.Value));
            }

            _reporter.LogStatus("Stopping MLA Application ...");
        }

        public void StopRead()
        {
            _cancellationTokenSource.Cancel();
        }

        public void AddDetectedObject(Hashtable detectedObjects, string detectedObject)
        {
            if (detectedObjects.ContainsKey(detectedObject))
            {
                int countName = (int)detectedObjects[detectedObject];
                detectedObjects[detectedObject] = countName + 1;//Increase the number of detected object
            }
            else
            {
                detectedObjects.Add(detectedObject, 1);//Add detected object
            }
        }

        public decimal GetProgress(long fileBytesLength, long position)
        {
            return (position * 100 / (decimal)fileBytesLength);
        }
    }
}