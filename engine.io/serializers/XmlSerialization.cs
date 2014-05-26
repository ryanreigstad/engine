using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using engine.data.exceptions;

namespace engine.io.serializers
{
    internal static class XmlSerialization<T>
        where T : ISerializable
    {
        private static XmlSerializer Serializer { get { return new XmlSerializer(typeof(T)); } }

        public static void SaveXml(FileInfo file, T data)
        {
            using (var s = new StreamWriter(file.FullName))
            {
                Serializer.Serialize(s, data);
            }
        }

        public static T LoadXml(FileInfo file)
        {
            if (!file.Exists)
                throw new EngineException(StandardExceptions.NewFileNotFound(file));

            using (var s = new StreamReader(file.FullName))
            {
                return (T) Serializer.Deserialize(s);
            }
        }
    }
}
