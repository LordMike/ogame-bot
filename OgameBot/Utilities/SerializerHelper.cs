using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace OgameBot.Utilities
{
    public static class SerializerHelper
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static void SerializeToStream<T>(T obj, Stream target)
        {
            using (StreamWriter sw = new StreamWriter(target, Encoding))
                Serializer.Serialize(sw, obj);
        }

        public static byte[] SerializeToBytes<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SerializeToStream(obj, ms);

                return ms.ToArray();
            }
        }

        public static T DeserializeFromStream<T>(Stream source)
        {
            using (StreamReader sr = new StreamReader(source, Encoding))
            using (JsonTextReader jr = new JsonTextReader(sr))
                return Serializer.Deserialize<T>(jr);
        }

        public static T DeserializeFromBytes<T>(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
                return DeserializeFromStream<T>(ms);
        }
    }
}