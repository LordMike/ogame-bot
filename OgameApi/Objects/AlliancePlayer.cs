using System.Xml.Serialization;

namespace OgameApi.Objects
{
    public partial class AlliancePlayer
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}