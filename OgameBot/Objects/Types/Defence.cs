namespace OgameBot.Objects.Types
{
    public class Defence : BaseEntityType<DefenceType, Defence>
    {
        public static Defence RocketLauncher { get; } = new Defence(DefenceType.RocketLauncher);
        public static Defence LightLaser { get; } = new Defence(DefenceType.LightLaser);
        public static Defence HeavyLaser { get; } = new Defence(DefenceType.HeavyLaser);
        public static Defence GaussCannon { get; } = new Defence(DefenceType.GaussCannon);
        public static Defence IonCannon { get; } = new Defence(DefenceType.IonCannon);
        public static Defence PlasmaTurret { get; } = new Defence(DefenceType.PlasmaTurret);
        public static Defence SmallShieldDome { get; } = new Defence(DefenceType.SmallShieldDome);
        public static Defence LargeShieldDome { get; } = new Defence(DefenceType.LargeShieldDome);

        private Defence(DefenceType type) : base(type)
        {
        }

        public static implicit operator Defence(DefenceType type)
        {
            return Index[type];
        }

        public static implicit operator DefenceType(Defence type)
        {
            return type.Type;
        }
    }
}