using System;
using System.IO;
using engine.data.exceptions;
using Newtonsoft.Json;

namespace engine.io.serializers
{
    internal static class JsonSerialization
    {
        private static readonly JsonSerializer Json = JsonSerializer.CreateDefault();

        public static void SaveJson(FileInfo file, object data)
        {
            using (var sw = new StreamWriter(file.FullName))
            using (var js = new JsonTextWriter(sw))
            {
                Json.Serialize(js, data);
            }
        }

        public static void SaveJson<T>(FileInfo file, T data)
        {
            SaveJson(file, (object) data);
        }

        public static object LoadJson(FileInfo file, Type t)
        {
            if (!file.Exists)
                throw new EngineException(StandardExceptions.NewFileNotFound(file));

            using (var sr = new StreamReader(file.FullName))
            using (var jr = new JsonTextReader(sr))
            {
                return Json.Deserialize(jr, t);
            }
        }

        public static T LoadJson<T>(FileInfo file)
        {
            return (T) LoadJson(file, typeof (T));
        }
    }
}
