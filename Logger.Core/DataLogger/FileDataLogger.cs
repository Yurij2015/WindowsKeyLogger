using System.IO;

namespace Logger.Core.DataLogger
{
    public class FileDataLogger : IDataLogger
    {
        private readonly string _filePath;

        public FileDataLogger()
        {
            _filePath = Path.GetTempFileName();
        }

        public void Write(string data)
        {
            using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(data);
                }
            }
        }
    }
}
