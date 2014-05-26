using System;
using engine.util.extensions;


namespace engine.data.config
{
    /// <summary>
    /// If we decide that we want to autosave configs
    /// </summary>
    public interface INotifyConfigChanged
    {
        event OnConfigChanged OnChanged;
    }

    public delegate void OnConfigChanged(ConfigChangedEventArgs args);

    public class ConfigChangedEventArgs : EventArgs
    {
        public ConfigChangedEventArgs(object sender, string name, object oldvalue, object newvalue)
        {
            Sender = sender;
            Name = name;
            Oldvalue = oldvalue;
            Newvalue = newvalue;
        }

        public object Sender { get; set; }
        public string Name { get; set; }
        public object Oldvalue { get; set; }
        public object Newvalue { get; set; }

        public override string ToString()
        {
            return "{0}:\n{1}.{2} old = {3}\n{1}.{2} new = {4}".Formatted(GetType().Name, Sender.GetType().Name, Name, Oldvalue, Newvalue);
        }
    }
}
