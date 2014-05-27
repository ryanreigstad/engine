using System;
using System.IO;
using System.Linq;
using engine.data.exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace engine.io.serializers
{
    internal static class JsonSerialization
    {
        private static readonly JsonSerializer Json = JsonSerializer.CreateDefault(
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    SerializeCompilerGeneratedMembers = false
                },
#if DEBUG
                Formatting = Formatting.Indented,
#endif
            });

        public static void SaveJson(FileInfo file, object data)
        {
            if (file.Directory != null && !file.Directory.Exists)
                file.Directory.Create();

            using (var f = file.Open(FileMode.OpenOrCreate, FileAccess.Write))
            using (var s = new StreamWriter(f))
            using (var js = new JsonTextWriter(s))
            {
                Json.Serialize(js, data);
                js.Flush();
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

            using (var f = file.Open(FileMode.Open, FileAccess.Read))
            using (var s = new StreamReader(f))
            using (var jr = new JsonTextReader(s))
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
