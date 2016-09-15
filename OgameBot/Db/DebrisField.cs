using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OgameBot.Db.Parts;
using OgameBot.Objects;

namespace OgameBot.Db
{
    public class DebrisField
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LocationId { get; set; }

        public Coordinate Coordinate
        {
            get { return CoordHelper.GetCoordinate(LocationId); }
            set { LocationId = CoordHelper.ToNumber(value); }
        }

        public DbResources Resources { get; set; }

        public DateTimeOffset LastSeen { get; set; }

        public DebrisField()
        {
            Resources = new DbResources();
        }

        public override string ToString()
        {
            return $"DebrisField {Coordinate}, id: {LocationId}, {(Resources)Resources}";
        }
    }
}