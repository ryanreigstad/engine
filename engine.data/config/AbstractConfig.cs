using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using engine.data.exceptions;
using engine.data.exceptions.logging;
using engine.util.extensions;
using Newtonsoft.Json;

namespace engine.data.config
{
    /// <summary>
    /// This class is how you have engine.io.config.ConfigManager load and save your config objects.
    /// See <see cref="SampleConfig"/> for details.
    /// 
    /// I did write <seealso cref="AbstractSingletonConfig"/> and <seealso cref="SingletonConfigManager"/> first though, so read that first becaue the comments might make more sense.
    /// </summary>
    [JsonObject(MemberSerialization.Fields)]
    public abstract class AbstractConfig : INotifyConfigChanged
    {
        protected AbstractConfig()
        {
            OnChanged += OnChangeNotified;
        }
        ~AbstractConfig()
        {
            OnChanged -= OnChangeNotified;
        }

        protected AbstractConfig(FileInfo file)
            : this()
        {
            File = file;
        }

        public event OnConfigChanged OnChanged;
        public bool Changed { get; set; }
        public FileInfo File { get; set; }

        public void Commit()
        {
            Changed = false;
        }

        public abstract AbstractConfig GetDefaults(FileInfo file);

        protected void NotifyOnChanged(object oldvalue, object newvalue, [CallerMemberName] string name = null)
        {
            if (OnChanged != null)
                OnChanged(new ConfigChangedEventArgs(this, name, oldvalue, newvalue));
        }

        protected void DefaultSet(object value, [CallerMemberName] string name = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ImplementationException("DefaultSet doesn't get the name of the caller member.");
            if (!name.Contains("_"))
                name = "_" + name.Substring(0, 1).ToLower() + name.Substring(1);

            var f = GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f == null)
                throw new ImplementationException("CallerMemberName '{0}' didn't translate to a field.".Formatted(name));

            var oldvalue = f.GetValue(this);
            if (!oldvalue.Equals(value))
            {
                f.SetValue(this, value);
                NotifyOnChanged(oldvalue, value, name);
            }
        }

        private void OnChangeNotified(ConfigChangedEventArgs args)
        {
            Changed = true;
            Logger.Log(args);
        }
    }

    /// <summary>
    /// This is a sample class demonstrating how much harder singletons and static can make things.
    /// 
    /// This is a Config object.
    /// </summary>
    public class SampleConfig : AbstractConfig
    {
        /// <summary>
        /// Notice the param.
        /// </summary>
        /// <param name="file">
        /// We tell the config what files it's going to be using.
        /// Compared to AbstractSingletonConfig, this is so much simpler
        /// </param>
        public SampleConfig(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// What are the defaults for this object?
        /// you don't have to supply a full object. most configs that aren't singleton configs are probably going to vary a lot.
        /// </summary>
        /// <param name="file">the file the defaults are being created against.</param>
        /// <returns>The defaults with null values where there is no default.</returns>
        /// <remarks>this is mainly used by ConfigManager to merge any missing info that we can.</remarks>
        public override AbstractConfig GetDefaults(FileInfo file)
        {
            return GetDefaultInstance(file);
        }

        /// <summary>
        /// I like to have the above as a static method.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static SampleConfig GetDefaultInstance(FileInfo file)
        {
            var index = 0;
            var defaultt = new SampleConfig(file)
            {
                _foo = "hello",
                _bar = ++index
            };
            var current = defaultt;
            for (; index < 100; index++)
            {
                current._inception = new SampleConfig(file)
                {
                    _foo = "hello",
                    _bar = ++index
                };
                current = current._inception;
            }
            return defaultt;
        }

        /// <summary>
        /// instance fields are serialized into/from json/xml.
        /// </summary>
        private string _foo;
        /// <summary>
        /// not properties though
        /// </summary>
        public string Foo
        {
            get { return _foo; }
            set
            {
                // more OnChanged stuff
                if (_foo != value)
                {
                    string oldvalue = _foo, newvalue = value;
                    _foo = newvalue;
                    NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        private double _bar;
        public double Bar
        {
            get { return _bar; }
            set
            {
                // floating point precision epsilon thing
                if (Math.Abs(_bar - value) > 0.0001d)
                {
                    double oldvalue = _bar, newvalue = value;
                    _bar = newvalue;
                    NotifyOnChanged(oldvalue, newvalue);
                }
            }
        }

        /// <summary>
        /// How many layers are you willing to go? 1024? 4096? 65536?!
        /// </summary>
        private SampleConfig _inception;
        public SampleConfig Inception
        {
            get { return _inception; }
            // same as in AbstractSingletonConfig, we have default set untested.
            set { DefaultSet(value); }
        }

        // You can have as many more fields as you want. Just make sure that you only have instance fields for the things that you want to be persisted.
    }
}
