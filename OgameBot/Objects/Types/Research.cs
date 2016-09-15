namespace OgameBot.Objects.Types
{
    public class Research : BaseEntityType<ResearchType, Research>
    {
        public static Research ArmourTechnology { get; } = new Research(ResearchType.ArmourTechnology);
        public static Research Astrophysics { get; } = new Research(ResearchType.Astrophysics);
        public static Research CombustionDrive { get; } = new Research(ResearchType.CombustionDrive);
        public static Research ComputerTechnology { get; } = new Research(ResearchType.ComputerTechnology);
        public static Research EnergyTechnology { get; } = new Research(ResearchType.EnergyTechnology);
        public static Research EspionageTechnology { get; } = new Research(ResearchType.EspionageTechnology);
        public static Research GravitonTechnology { get; } = new Research(ResearchType.GravitonTechnology);
        public static Research HyperspaceDrive { get; } = new Research(ResearchType.HyperspaceDrive);
        public static Research HyperspaceTechnology { get; } = new Research(ResearchType.HyperspaceTechnology);
        public static Research ImpulseDrive { get; } = new Research(ResearchType.ImpulseDrive);
        public static Research IntergalacticResearchNetwork { get; } = new Research(ResearchType.IntergalacticResearchNetwork);
        public static Research IonTechnology { get; } = new Research(ResearchType.IonTechnology);
        public static Research LaserTechnology { get; } = new Research(ResearchType.LaserTechnology);
        public static Research PlasmaTechnology { get; } = new Research(ResearchType.PlasmaTechnology);
        public static Research ShieldingTechnology { get; } = new Research(ResearchType.ShieldingTechnology);
        public static Research WeaponsTechnology { get; } = new Research(ResearchType.WeaponsTechnology);

        private Research(ResearchType type) : base(type)
        {
        }

        public static implicit operator Research(ResearchType type)
        {
            return Index[type];
        }

        public static implicit operator ResearchType(Research type)
        {
            return type.Type;
        }
    }
}