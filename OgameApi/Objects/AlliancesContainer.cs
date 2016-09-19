using System.Xml.Serialization;

namespace OgameApi.Objects
{
    [XmlRoot("alliances")]
    public partial class AlliancesContainer
    {
        [XmlElement("alliance", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AllianceInfo[] Alliances { get; set; }

        [XmlAttribute("timestamp")]
        public int Timestamp { get; set; }

        [XmlAttribute("serverId")]
        public string ServerId { get; set; }
    }
}