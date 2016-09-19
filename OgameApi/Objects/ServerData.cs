using System.Xml.Serialization;

namespace OgameApi.Objects
{
    [XmlRoot("serverData")]
    public partial class ServerData 
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("language")]
        public string Language { get; set; }

        [XmlElement("timezone")]
        public string Timezone { get; set; }

        [XmlElement("domain")]
        public string Domain { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("speed")]
        public int Speed { get; set; }

        [XmlElement("galaxies")]
        public int Galaxies { get; set; }

        [XmlElement("systems")]
        public int Systems { get; set; }

        [XmlElement("acs")]
        public bool Acs { get; set; }

        [XmlElement("rapidFire")]
        public bool RapidFire { get; set; }

        [XmlElement("defToTF")]
        public bool DefenseToDebrisField { get; set; }

        [XmlElement("debrisFactor")]
        public float DebrisFactor { get; set; }

        [XmlElement("repairFactor")]
        public float RepairFactor { get; set; }

        [XmlElement("newbieProtectionLimit")]
        public int NewbieProtectionLimit { get; set; }

        [XmlElement("newbieProtectionHigh")]
        public int NewbieProtectionHigh { get; set; }

        [XmlElement("topScore")]
        public int TopScore { get; set; }

        [XmlAttribute("timestamp")]
        public int Timestamp { get; set; }

        [XmlAttribute("serverId")]
        public string ServerId { get; set; }
    }
}
