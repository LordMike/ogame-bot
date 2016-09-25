using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace OgameBot.Utilities
{
    public static class SerializerHelper
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static void SerializeToStream<T>(T obj, Stream target, bool compress = false)
        {
            Stream pseudoTarget = target;

            if (compress)
                pseudoTarget = new GZipStream(pseudoTarget, CompressionLevel.Optimal);

            using (StreamWriter sw = new StreamWriter(pseudoTarget, Encoding))
                Serializer.Serialize(sw, obj);
        }

        public static byte[] SerializeToBytes<T>(T obj, bool compress = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SerializeToStream(obj, ms, compress);
                return ms.ToArray();
            }
        }

        public static T DeserializeFromStream<T>(Stream source, bool decompress = false)
        {
            return (T)DeserializeFromStream(typeof(T), source, decompress);
        }

        private static object DeserializeFromStream(Type type, Stream source, bool decompress = false)
        {
            Stream pseudoSource = source;
            if (decompress)
                pseudoSource = new GZipStream(pseudoSource, CompressionMode.Decompress);

            using (StreamReader sr = new StreamReader(pseudoSource, Encoding))
            using (JsonTextReader jr = new JsonTextReader(sr))
                return Serializer.Deserialize(jr, type);
        }

        public static object DeserializeFromBytes(Type type, byte[] data, bool decompress = false)
        {
            using (MemoryStream ms = new MemoryStream(data))
                return DeserializeFromStream(type, ms, decompress);
        }

        public static T DeserializeFromBytes<T>(byte[] data, bool decompress = false)
        {
            using (MemoryStream ms = new MemoryStream(data))
                return (T)DeserializeFromStream(typeof(T), ms, decompress);
        }
    }
}