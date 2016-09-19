using System.Xml.Serialization;

namespace OgameApi.Objects
{
    public partial class Planet
    {
        [XmlElement("moon")]
        public Moon Moon { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("player")]
        public int Player { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("coords")]
        public string Coords { get; set; }
    }
}