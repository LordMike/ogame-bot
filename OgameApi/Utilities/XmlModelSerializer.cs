using System.IO;
using System.Xml.Serialization;

namespace OgameApi.Utilities
{
    public static class XmlModelSerializer
    {
        public static T Deserialize<T>(FileInfo file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var fs = File.OpenRead(file.FullName))
                return (T)serializer.Deserialize(fs);
        }
    }
}
