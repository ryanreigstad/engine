using engine.data.exceptions;
using engine.util.extensions;

namespace engine.data.config
{
    public class LoggerConfig : AbstractSingletonConfig
    {
        private const string Filename = "logger.config";

        private static LoggerConfig _instance;
        private static LoggerConfig i
        {
            get
            {
                if (_instance == null)
                    throw new ImplementationException("You cannot access config files ('{0}') before they have been loaded.".Formatted(Filename));
                return _instance;
            }
        }
        private static LoggerConfig GetDefaultInstance()
        {
            return new LoggerConfig
            {
                _enabled = true
            };
        }

        private LoggerConfig()
            : base()
        {
        }

        private bool _enabled;
        public static bool Enabled
        {
            get { return i._enabled; }
            set
            {
                if (i._enabled != value)
                {
                    bool oldvalue = i._enabled, newvalue = value;
                    i._enabled = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
                // DefaultSet(i, value);
            }
        }
    }
}
