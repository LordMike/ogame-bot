using System.Xml.Serialization;

namespace OgameApi.Objects
{
    [XmlRoot("players")]
    public partial class PlayersContainer
    {
        [XmlElement("player", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Player[] Players { get; set; }

        [XmlAttribute("timestamp")]
        public int Timestamp { get; set; }

        [XmlAttribute("serverId")]
        public string ServerId { get; set; }
    }
}