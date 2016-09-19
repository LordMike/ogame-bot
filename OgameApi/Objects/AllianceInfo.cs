using System.Xml.Serialization;

namespace OgameApi.Objects
{
    public partial class AllianceInfo
    {
        [XmlElement("player", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AlliancePlayer[] Player { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tag")]
        public string Tag { get; set; }

        [XmlAttribute("homepage")]
        public string Homepage { get; set; }

        [XmlAttribute("logo")]
        public string Logo { get; set; }

        [XmlAttribute("open")]
        public bool Open { get; set; }

        [XmlIgnore]
        public bool OpenSpecified { get; set; }
    }
}