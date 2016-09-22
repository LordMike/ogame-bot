using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OgameBot.Objects;

namespace OgameBot.Db
{
    public class GalaxyScan : ICreatedOn
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LocationId { get; set; }

        public SystemCoordinate SystemCoordinate
        {
            get { return CoordHelper.GetSysCoordinate(LocationId); }
            set { LocationId = CoordHelper.ToNumber(value); }
        }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LastScan { get; set; }

        public override string ToString()
        {
            return $"GalaxyScan {SystemCoordinate}, id: {LocationId}";
        }
    }
}