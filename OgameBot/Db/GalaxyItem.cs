using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OgameBot.Objects;

namespace OgameBot.Db
{
    public class GalaxyItem
    {
        private PlanetInfo _planetInfo;

        [Key]
        public long LocationId { get; set; }

        public Coordinate Coordinate
        {
            get { return CoordHelper.GetCoordinate(LocationId); }
            set { LocationId = CoordHelper.ToNumber(value); }
        }
        
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