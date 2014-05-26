using System;
using System.IO;
using System.Linq;
using System.Reflection;
using engine.data;
using engine.data.config;
using engine.data.exceptions;
using engine.io.serializers;
using engine.util.extensions;

namespace engine.io.config
{
    public static class SingletonConfigManager
    {
        private static Type[] _singletonConfigTypes;

        /// <summary>
        /// Loads the Config Types by looking at every loaded class.
        /// </summary>
        static SingletonConfigManager()
        {
            ReadConfigTypes();
#if DEBUG
            // Make sure all the required code elements are present in every config class. 
            foreach (var config in _singletonConfigTypes)
            {
                // Config Filename
                var filename = config.GetField("Filename", BindingFlags.NonPublic | BindingFlags.Static);
                if (filename == null)
                    throw new ImplementationException("'{0}' does not implement nonpublic static field 'Filename'."
                                                            .Formatted(config.FullName));
                if (filename.FieldType != typeof(string))
                    throw new ImplementationException("'{0}' implements 'Filename' as a '{1}' instead of a '{2}'."
                                                            .Formatted(config.FullName, filename.FieldType.FullName, typeof(string).FullName));
                // Config Instance
                var instance = config.GetField("_instance", BindingFlags.NonPublic | BindingFlags.Static);
                if (instance == null)
                    throw new ImplementationException("'{0}' does not implement nonpublic static field '_instance'."
                                                            .Formatted(config.FullName));
                if (instance.FieldType != config)
                    throw new ImplementationException("'{0}' implements '_instance' as a {1} instead of a '{0}'."
                                                            .Formatted(config.FullName, filename.FieldType.FullName));
                // Config Default Instance
                var method = config.GetMethod("GetDefaultInstance", BindingFlags.NonPublic | BindingFlags.Static);
                if (method == null)
                    throw new ImplementationException("'{0}' does not implement nonpublic static method 'GetDefaultInstance'."
                                                            .Formatted(config.FullName));
                if (method.ReturnType != config)
                    throw new ImplementationException("'{0}' defines the return type of 'GetDefaultInstance' as '{1}' instead of '{0}'."
                                                            .Formatted(config.FullName, method.ReturnType.FullName));
                if (method.GetParameters().Length != 0)
                    throw new ImplementationException("'{0}' defines arguments to 'GetDefaultInstance'. Remove them."
                                                            .Formatted(config.FullName));
            }
#endif
        }

        /// <summary>
        /// Looks at all of the class declarations in all of the loaded assemblies for subclasses of <see cref="AbstractSingletonConfig"/>
        /// </summary>
        public static void ReadConfigTypes()
        {
            _singletonConfigTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(typeof(AbstractSingletonConfig))).ToArray();
        }

        #region Load Config

        /// <summary>
        /// Finds any empty fields in the current config instance and assigns the value from GetDefaultInstance().
        /// </summary>
        /// <param name="t">The config type to perform the merge for.</param>
        /// <remarks>Will only merge top level configurations at the moment.</remarks>
        private static void MergeDefaultConfig(Type t)
        {
            var instance = Get_Instance(t);

            var unset = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.GetValue(instance).Equals(f.FieldType.IsValueType ? Activator.CreateInstance(f.FieldType) : null)).ToList();
            if (unset.Count == 0)
                return;

            var defaultt = Invoke_GetDefaultInstance(t);
            foreach (var field in unset)
                field.SetValue(instance, field.GetValue(defaultt));

            SaveConfig(Filepaths.GetConfigFileInfo(Get_Filename(t)), instance, true);
        }

        /// <summary>
        /// Loads and merges the default values or creates the file from the default values if it doesn't exist.
        /// </summary>
        /// <param name="file">The file to load/create.</param>
        /// <param name="t">The config type.</param>
        private static void LoadConfig(FileInfo file, Type t)
        {
            if (file.Exists)
            {
                Set_Instance(t, (AbstractSingletonConfig) JsonSerialization.LoadJson(file, t));
                MergeDefaultConfig(t);
            }
            else
            {
                Set_Instance(t, Invoke_GetDefaultInstance(t));
                SaveConfig(file, Get_Instance(t));
            }
        }

        /// <summary>
        /// Load a config file based on the supplied type.
        /// </summary>
        /// <param name="t">Type of the config file to load.</param>
        public static void LoadConfig(Type t)
        {
            LoadConfig(Filepaths.GetConfigFileInfo(Get_Filename(t)), t);
        }

        /// <summary>
        /// Load a config file based on the supplied type.
        /// </summary>
        /// <typeparam name="T">Type of the config file to load.</typeparam>
        public static void LoadConfig<T>()
            where T : AbstractSingletonConfig
        {
            LoadConfig(typeof (T));
        }

        /// <summary>
        /// Load all the known singleton config types.
        /// </summary>
        public static void LoadAll()
        {
            _singletonConfigTypes.ToList().ForEach(LoadConfig);
        }

        #endregion

        #region Save Config

        /// <summary>
        /// Send a file off to serialization.
        /// </summary>
        /// <param name="file">The file to serialize to.</param>
        /// <param name="config">The config to serialize.</param>
        /// <param name="force">Don't care that it's clean data?</param>
        private static void SaveConfig(FileInfo file, AbstractSingletonConfig config, bool force = false)
        {
            if (config.Changed || force)
                JsonSerialization.SaveJson(file, config);
            config.Commit();
        }

        /// <summary>
        /// Save a config file based on the supplied type.
        /// </summary>
        /// <param name="t">Type of the config file to save.</param>
        public static void SaveConfig(Type t)
        {
            SaveConfig(Filepaths.GetConfigFileInfo(Get_Filename(t)), Get_Instance(t));
        }

        /// <summary>
        /// Save a config file based on the supplied type.
        /// </summary>
        /// <typeparam name="T">Type of the config file to save.</typeparam>
        public static void SaveConfig<T>()
            where T : AbstractSingletonConfig
        {
            SaveConfig(typeof (T));
        }

        /// <summary>
        /// Save all the known singleton config types.
        /// </summary>
        public static void SaveAll()
        {
            _singletonConfigTypes.ToList().ForEach(SaveConfig);
        }

        #endregion

        #region Shitty Reflection Utils

        /// <summary>
        /// Gets the static 'Filename' from the config type.
        /// </summary>
        /// <param name="t">The config type.</param>
        /// <returns>The string Filename.</returns>
        private static string Get_Filename(Type t)
        {
            return (string) t.GetField("Filename", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        }

        /// <summary>
        /// Gets the current config '_instance' from the config type.
        /// </summary>
        /// <param name="t">The config type.</param>
        /// <returns>The current Instance of the config object.</returns>
        private static AbstractSingletonConfig Get_Instance(Type t)
        {
            return (AbstractSingletonConfig) t.GetField("_instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        }

        /// <summary>
        /// Sets the value of the currect config '_instance' for the config type.
        /// </summary>
        /// <param name="t">The config type.</param>
        /// <param name="value">The value to set.</param>
        private static void Set_Instance(Type t, AbstractSingletonConfig value)
        {
            t.GetField("_instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
        }

        /// <summary>
        /// Gets the default config object from the config type.
        /// </summary>
        /// <param name="t">The config type.</param>
        /// <returns>The default config object</returns>
        private static AbstractSingletonConfig Invoke_GetDefaultInstance(Type t)
        {
            return (AbstractSingletonConfig) t.GetMethod("GetDefaultInstance", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]);
        }

        #endregion
    }
}
