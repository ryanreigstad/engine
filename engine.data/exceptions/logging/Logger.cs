using System;
using engine.data.config;

namespace engine.data.exceptions.logging
{
    public static class Logger
    {
        public static void Log(string message)
        {
            if (!LoggerConfig.Enabled)
                return;

            Console.WriteLine(message);
        }

        public static void Log(object data)
        {
            Log(data.ToString());
        }
    }

    public static class LoggerExtensions
    {
        public static void Log<T>(this T exception)
            where T : Exception
        {
            if (!LoggerConfig.Enabled)
                return;

            Console.Error.WriteLine(exception);
        }
    }
}
