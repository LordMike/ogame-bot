using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OgameBot.Objects.Types;

namespace OgameBot.Db
{
    public class GalaxyItem
    {
        private PlanetInfo _planetInfo;

        [Key, Column(Order = 0)]
        public byte Galaxy { get; set; }

        [Key, Column(Order = 1)]
        public short System { get; set; }

        [Key, Column(Order = 2)]
        public byte Planet { get; set; }

        [Key, Column(Order = 3)]
        public CoordinateType Type { get; set; }

        /// <summary>
        /// Internal. Don't touch this
        /// </summary>
        public byte[] PlanetInfoData { get; set; }

        [NotMapped]
        public PlanetInfo PlanetInfo
        {
            get
            {
                if (_planetInfo == null)
                {
                    if (PlanetInfoData != null)
                        _planetInfo = SerializerHelper.DeserializeFromBytes<PlanetInfo>(PlanetInfoData);
                    else
                        _planetInfo = new PlanetInfo();
                }

                return _planetInfo;
            }
        }

        public DbResources Resources { get; set; }

        public DateTimeOffset LastResourcesTime { get; set; }

        public DateTimeOffset LastBuildingsTime { get; set; }

        public DateTimeOffset LastShipsTime { get; set; }

        public DateTimeOffset LastDefencesTime { get; set; }

        public void Update()
        {
            PlanetInfoData = SerializerHelper.SerializeToBytes(PlanetInfo);
        }
    }
}