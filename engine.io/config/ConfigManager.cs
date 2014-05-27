using System.IO;
using engine.data.config;
using engine.io.serializers;
using engine.util;

namespace engine.io.config
{
    /// <summary>
    /// A slimmed down version of <see cref="SingletonConfigManager"/> meant for nonsingletons.
    /// Read that for details and everything here will make sense. (I wrote that first, the comments are better)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ConfigManager<T>
        where T : AbstractConfig
    {
        public static T LoadConfig(FileInfo file)
        {
            var t = JsonSerialization.LoadJson<T>(file);
            t.File = file;
            ReflectionUtils.Merge(t, t.GetDefaults(file), true, false);
            return t;
        }

        public static void SaveConfig(T config, bool force = false)
        {
            if (config.Changed || force)
                JsonSerialization.SaveJson(config.File, config);
            config.Commit();
        }
    }
}
