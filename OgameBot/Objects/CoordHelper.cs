using OgameBot.Objects.Types;

namespace OgameBot.Objects
{
    public static class CoordHelper
    {
        public static long ToNumber(Coordinate coord)
        {
            // Gal  | Systm | Plnt | Type
            // byte | short | byte | byte

            return ((long)coord.Galaxy << 32) + (coord.System << 16) +
                   (coord.Planet << 8) + (byte)coord.Type;
        }

        public static int ToNumber(SystemCoordinate coord)
        {
            // Gal  | Systm
            // byte | short

            return (coord.Galaxy << 16) + coord.System;
        }

        public static bool IsSystemCoordinate(long id)
        {
            // If it's a long, it's a planet coordinate
            return id < int.MaxValue;
        }

        public static Coordinate GetCoordinate(long id)
        {
            byte gal = (byte)((id >> 32) & 0xFF);
            short sys = (short)((id >> 16) & 0xFFFF);
            byte pln = (byte)((id >> 8) & 0xFF);
            CoordinateType typ = (CoordinateType)(id & 0xFF);

            return new Coordinate(gal, sys, pln, typ);
        }

        public static SystemCoordinate GetSysCoordinate(int id)
        {
            byte gal = (byte)((id >> 16) & 0xFF);
            short sys = (short)(id & 0xFFFF);

            return new SystemCoordinate(gal, sys);
        }
    }
}