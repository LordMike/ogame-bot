namespace OgameBot.Objects
{
    public struct SystemCoordinate
    {
        public byte Galaxy { get; set; }

        public short System { get; set; }

        public SystemCoordinate(byte galaxy, short system)
        {
            Galaxy = galaxy;
            System = system;
        }

        public static SystemCoordinate Create(SystemCoordinate baseSystemCoordinate, short system)
        {
            return new SystemCoordinate(baseSystemCoordinate.Galaxy, system);
        }

        public bool Equals(SystemCoordinate other)
        {
            return Galaxy == other.Galaxy && System == other.System;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is SystemCoordinate && Equals((SystemCoordinate)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Galaxy.GetHashCode();
                hashCode = (hashCode * 397) ^ System.GetHashCode();
                return hashCode;
            }
        }

        public int Id => this;

        public static implicit operator int(SystemCoordinate coord)
        {
            return CoordHelper.ToNumber(coord);
        }

        public static implicit operator SystemCoordinate(int id)
        {
            return CoordHelper.GetCoordinate(id);
        }

        public static implicit operator SystemCoordinate(Coordinate coord)
        {
            return new SystemCoordinate(coord.Galaxy, coord.System);
        }

        public static bool operator <(SystemCoordinate left, SystemCoordinate right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator >(SystemCoordinate left, SystemCoordinate right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator >=(SystemCoordinate left, SystemCoordinate right)
        {
            return Compare(left, right) >= 0;
        }

        public static bool operator <=(SystemCoordinate left, SystemCoordinate right)
        {
            return Compare(left, right) <= 0;
        }

        public static bool operator ==(SystemCoordinate left, SystemCoordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SystemCoordinate left, SystemCoordinate right)
        {
            return !left.Equals(right);
        }

        public static int Compare(SystemCoordinate x, SystemCoordinate y)
        {
            if (x.Galaxy != y.Galaxy)
                return x.Galaxy.CompareTo(y.Galaxy);

            return x.System.CompareTo(y.System);
        }

        public override string ToString()
        {
            return $"[{Galaxy}:{System}]";
        }
    }
}