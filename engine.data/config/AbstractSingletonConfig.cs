using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using engine.data.exceptions;
using engine.data.exceptions.logging;
using engine.util.extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace engine.data.config
{
    /// <summary>
    /// This class is how you have engine.io.config.SingletonConfigManager load and save your singleton config objects.
    /// See <see cref="SampleSingletonConfig"/> for details.
    /// </summary>
    [JsonObject(MemberSerialization.Fields)]
    public abstract class AbstractSingletonConfig : INotifyConfigChanged
    {
        protected AbstractSingletonConfig()
        {
            OnChanged += OnChangeNotified;
        }
        ~AbstractSingletonConfig()
        {
            OnChanged -= OnChangeNotified;
        }

        [field: JsonIgnore]
        public event OnConfigChanged OnChanged;
        [JsonIgnore] public bool Changed;

        public void Commit()
        {
            Changed = false;
        }

        protected void NotifyOnChanged(object oldvalue, object newvalue, [CallerMemberName] string name = null)
        {
            if (OnChanged != null)
                OnChanged(new ConfigChangedEventArgs(this, name, oldvalue, newvalue));
        }

        protected static void DefaultSet(AbstractSingletonConfig config, object value, [CallerMemberName] string name = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ImplementationException("DefaultSet doesn't get the name of the caller member.");
            if (!name.Contains("_"))
                name = "_" + name.Substring(0, 1).ToLower() + name.Substring(1);

            var f = config.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f == null)
                throw new ImplementationException("CallerMemberName '{0}' didn't translate to a field.".Formatted(name));

            var oldvalue = f.GetValue(config);
            if (!oldvalue.Equals(value))
            {
                f.SetValue(config, value);
                config.NotifyOnChanged(oldvalue, value, name);
            }
        }

        private void OnChangeNotified(ConfigChangedEventArgs args)
        {
            Changed = true;
            Logger.Log(args);
        }
    }

    #region How To: AbstractSingletonConfig and SingletonConfigManager
    /// <summary>
    /// This is a sample class to demonstrate how to create a singleton config object and have it work with <see cref="engine.io.config.SingletonConfigManager"/>.
    /// 
    /// I recommend copying and pasting this class and then changing the parts that are marked as data because this is pretty magical.
    /// </summary>
    internal class SampleSingletonConfig : AbstractSingletonConfig
    {
        // Required field. Must be a static field named 'Filename' and be an instance of string.
        // When you run the program, it will generate the default config and save it to this file. The path is to the engine.testificate folder.
        private const string Filename = "sample.config";

        // Required field. Must be a private static field named '_instance' and be of the same type as the config class.
        private static SampleSingletonConfig _instance;
        // Not Required. Generally useful though
        private static SampleSingletonConfig i
        {
            get
            {
                if (_instance == null)
                    throw new ImplementationException("You cannot access config files ('{0}') before they have been loaded.".Formatted(Filename));
                return _instance;
            }
            // no set. ConfigManager is the only thing that should set the program's config instance (notice that resharper thinks it never gets set).
            // ConfigManager will set directly to '_instance' even though it's private.
            // let everything else update the settings through the static properties.
        }

        /// <summary>
        /// Required Method. Must be a private static method named 'GetDefaultInstance' with no parameters and return an instance of the config class.
        /// </summary>
        /// <returns>The default values for the config object.</returns>
        private static SampleSingletonConfig GetDefaultInstance()
        {
            return new SampleSingletonConfig
            {
                _foo = "hello",
                _bar = (float) Math.PI / 2f,
                _magic = new {the = "magic", never = "stops"}
            };
        }

        /// <summary>
        /// These objects are singletons. No public constructors.
        /// </summary>
        private SampleSingletonConfig()
            : base()
        {
        }

        /// <summary>
        /// A Data. Must be an instance field.
        /// </summary>
        private string _foo;
        /// <summary>
        /// Static property to expose the current Instance's '_foo' configuration.
        /// </summary>
        public static string Foo
        {
            get { return i._foo; }
            set
            {
                // are we about to change the state of the config?
                if (i._foo != value)
                {
                    // grab the state change
                    string oldvalue = i._foo, newvalue = value;
                    // let it happen
                    i._foo = newvalue;
                    // let everyone know
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        /// <summary>
        /// Another Data.
        /// </summary>
        private float _bar;
        /// <summary>
        /// Another Exposer.
        /// </summary>
        public static float Bar
        {
            get { return i._bar; }
            set
            {
                // floating point error so epsilon
                if (Math.Abs(i._bar - value) > 0.001f)
                {
                    float oldvalue = i._bar, newvalue = value;
                    i._bar = newvalue;
                    i.NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        /// <summary>
        /// Through the magic of Json, you can have composite datas too.
        /// </summary>
        private dynamic _magic;
        // If you do it right, all your properties will look exactly the same with one or two name changes (like above).
        public static dynamic Magic
        {
            get { return i._magic; }
            // There is also a utility method that i'm not sure if it works yet... too much magic
            // but if it does work, than all these properties will look exactly like this one
            set { DefaultSet(i, value); }
        }

        // You can have as many more fields as you want. Just make sure that you only have instance fields for the things that you want to be persisted.
    }
    #endregion
}
