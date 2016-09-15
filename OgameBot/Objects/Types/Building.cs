namespace OgameBot.Objects.Types
{
    public class Building : BaseEntityType<BuildingType, Building>
    {
        public static Building AllianceDepot { get; } = new Building(BuildingType.AllianceDepot);
        public static Building CrystalMine { get; } = new Building(BuildingType.CrystalMine);
        public static Building CrystalStorage { get; } = new Building(BuildingType.CrystalStorage);
        public static Building DeuteriumSynthesizer { get; } = new Building(BuildingType.DeuteriumSynthesizer);
        public static Building DeuteriumTank { get; } = new Building(BuildingType.DeuteriumTank);
        public static Building FusionReactor { get; } = new Building(BuildingType.FusionReactor);
        public static Building JumpGate { get; } = new Building(BuildingType.JumpGate);
        public static Building LunarBase { get; } = new Building(BuildingType.LunarBase);
        public static Building MetalMine { get; } = new Building(BuildingType.MetalMine);
        public static Building MetalStorage { get; } = new Building(BuildingType.MetalStorage);
        public static Building MissileSilo { get; } = new Building(BuildingType.MissileSilo);
        public static Building NaniteFactory { get; } = new Building(BuildingType.NaniteFactory);
        public static Building ResearchLab { get; } = new Building(BuildingType.ResearchLab);
        public static Building RoboticFactory { get; } = new Building(BuildingType.RoboticFactory);
        public static Building SensorPhalanx { get; } = new Building(BuildingType.SensorPhalanx);
        //public static Building SpaceDock { get; } = new Building(BuildingType.SpaceDock);
        public static Building Shipyard { get; } = new Building(BuildingType.Shipyard);
        public static Building SolarPlant { get; } = new Building(BuildingType.SolarPlant);
        public static Building Terraformer { get; } = new Building(BuildingType.Terraformer);

        private Building(BuildingType type) : base(type)
        {
        }

        public static implicit operator Building(BuildingType type)
        {
            return Index[type];
        }

        public static implicit operator BuildingType(Building type)
        {
            return type.Type;
        }
    }
}