using System;

namespace Logger.Core.DataLogger
{
    public class ConsoleDataLogger : IDataLogger
    {
        public void Write(string data)
        {
            Console.Write(data);
        }
    }
}
