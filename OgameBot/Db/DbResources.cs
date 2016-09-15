using System.ComponentModel.DataAnnotations.Schema;
using OgameBot.Objects;

namespace OgameBot.Db
{
    [ComplexType]
    public class DbResources
    {
        public int Metal { get; set; }

        public int Crystal { get; set; }

        public int Deuterium { get; set; }

        public int Energy { get; set; }

        public static implicit operator Resources(DbResources type)
        {
            return new Resources
            {
                Metal = type.Metal,
                Crystal = type.Crystal,
                Deuterium = type.Deuterium,
                Energy = type.Energy
            };
        }

        public static implicit operator DbResources(Resources type)
        {
            return new DbResources
            {
                Metal = type.Metal,
                Crystal = type.Crystal,
                Deuterium = type.Deuterium,
                Energy = type.Energy
            };
        }
    }
}