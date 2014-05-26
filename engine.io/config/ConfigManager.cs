using System;
using System.IO;
using System.Linq;
using System.Reflection;
using engine.data.config;
using engine.io.serializers;

namespace engine.io.config
{
    /// <summary>
    /// A slimmed down version of <see cref="SingletonConfigManager"/> not meant for singletons.
    /// Read that for details and everything here will make sense.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ConfigManager<T>
        where T : AbstractConfig
    {
        public static T LoadConfig(FileInfo file)
        {
            var t = JsonSerialization.LoadJson<T>(file);
            t.File = file;
            return MergeDefaultConfig(t);
        }

        private static T MergeDefaultConfig(T instance)
        {
            var unset = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.GetValue(instance).Equals(f.FieldType.IsValueType ? Activator.CreateInstance(f.FieldType) : null)).ToList();
            if (unset.Count == 0)
                return instance;

            var defaultt = instance.GetDefaults(instance.File);
            foreach (var field in unset)
                field.SetValue(instance, field.GetValue(defaultt));

            SaveConfig(instance, true);
            return instance;
        }

        public static void SaveConfig(T config, bool force = false)
        {
            if (config.Changed || force)
                JsonSerialization.SaveJson(config.File, config);
            config.Commit();
        }
    }
}
