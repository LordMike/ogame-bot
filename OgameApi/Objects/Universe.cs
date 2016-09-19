using System.Xml.Serialization;

namespace OgameApi.Objects
{
    [XmlRoot("universe")]
    public partial class Universe
    {
        [XmlElement("planet", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Planet[] Planets { get; set; }

        [XmlAttribute("timestamp")]
        public int Timestamp { get; set; }

        [XmlAttribute("serverId")]
        public string ServerId { get; set; }
    }
}