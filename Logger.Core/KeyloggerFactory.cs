using System;
using Logger.Core.DataLogger;

namespace Logger.Core
{
    public class KeyloggerFactory
    {
        public static Keylogger GetKeylogger(OutputType logType)
        {
            switch (logType)
            {
                case OutputType.Console:
                    return new Keylogger(new ConsoleDataLogger());

                case OutputType.File:
                    return new Keylogger(new FileDataLogger());
            }

            throw new NotImplementedException($"Logging type of {logType.ToString("G")} is not supported.");
        }
    }
}