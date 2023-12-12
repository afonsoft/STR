using System;
using Eaf.Dependency;
using Eaf.Timing;
using Castle.Core.Logging;

namespace Eaf.Str.Migrator
{
    public class Log : ITransientDependency
    {
        public ILogger Logger { get; set; }

        public Log()
        {
            Logger = NullLogger.Instance;
        }

        public void Write(string text, bool breakline = true)
        {
            if (breakline)
                Console.WriteLine(Clock.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + text);
            else
                Console.Write(Clock.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + text);
            Logger.Info(text);
        }
    }
}