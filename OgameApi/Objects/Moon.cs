using System.Xml.Serialization;

namespace OgameApi.Objects
{
    public partial class Moon
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }
    }
}