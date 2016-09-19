using System.Xml.Serialization;

namespace OgameApi.Objects
{
    public partial class Player
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("status")]
        public string Status { get; set; }

        [XmlAttribute("alliance")]
        public int Alliance { get; set; }
    }
}