using System;
using System.ComponentModel.DataAnnotations;
using OgameBot.Objects;

namespace OgameBot.Db
{
    public class GalaxyScan
    {
        [Key]
        public int LocationId { get; set; }

        public SystemCoordinate SystemCoordinate
        {
            get { return CoordHelper.GetCoordinate(LocationId); }
            set { LocationId = CoordHelper.ToNumber(value); }
        }

        public DateTimeOffset LastScan { get; set; }
    }
}