namespace OgameBot.Objects.Types
{
    public class Ship : BaseEntityType<ShipType, Ship>
    {
        public static Ship LightFighter { get; } = new Ship(ShipType.LightFighter);
        public static Ship HeavyFighter { get; } = new Ship(ShipType.HeavyFighter);
        public static Ship Cruiser { get; } = new Ship(ShipType.Cruiser);
        public static Ship Battleship { get; } = new Ship(ShipType.Battleship);
        public static Ship Battlecruiser { get; } = new Ship(ShipType.Battlecruiser);
        public static Ship Bomber { get; } = new Ship(ShipType.Bomber);
        public static Ship Destroyer { get; } = new Ship(ShipType.Destroyer);
        public static Ship Deathstar { get; } = new Ship(ShipType.Deathstar);
        public static Ship SmallCargo { get; } = new Ship(ShipType.SmallCargo);
        public static Ship LargeCargo { get; } = new Ship(ShipType.LargeCargo);
        public static Ship Colony { get; } = new Ship(ShipType.ColonyShip);
        public static Ship Recycler { get; } = new Ship(ShipType.Recycler);
        public static Ship EspionageProbe { get; } = new Ship(ShipType.EspionageProbe);
        public static Ship SolarSatellite { get; } = new Ship(ShipType.SolarSatellite);

        private Ship(ShipType type) : base(type)
        {
        }

        public static implicit operator Ship(ShipType type)
        {
            return Index[type];
        }

        public static implicit operator ShipType(Ship type)
        {
            return type.Type;
        }
    }
}