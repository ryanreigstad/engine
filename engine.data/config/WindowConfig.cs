using engine.data.exceptions;
using engine.util.extensions;

namespace engine.data.config
{
    public class WindowConfig : AbstractSingletonConfig
    {
        private const string Filename = "window.config";

        private static WindowConfig _instance;
        private static WindowConfig i
        {
            get
            {
                if (_instance == null)
                    throw new ImplementationException("You cannot access config files ('{0}') before they have been loaded.".Formatted(Filename));
                return _instance;
            }
        }

        private static WindowConfig GetDefaultInstance()
        {
            return new WindowConfig
            {
                _windowTitle = "Game Engine Mk 0.0.0.2",
                _screenSize = new Vector2i(1600, 900)
            };
        }

        private WindowConfig()
            : base()
        {
        }

        private string _windowTitle;
        public static string WindowTitle
        {
            get { return i._windowTitle; }
            set
            {
                if (i._windowTitle != value)
                {
                    string oldvalue = i._windowTitle, newvalue = value;
                    i._windowTitle = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private Vector2i _screenSize;
        public static Vector2i ScreenSize
        {
            get { return i._screenSize; }
            set
            {
                if (i._screenSize != value)
                {
                    Vector2i oldvalue = i._screenSize, newvalue = value;
                    i._screenSize = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }
    }
}
