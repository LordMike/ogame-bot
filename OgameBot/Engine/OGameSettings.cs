using System;

namespace OgameBot.Engine
{
    public class OGameSettings
    {
        public TimeSpan ServerUtcOffset { get; set; }

        public byte Galaxies { get; set; }

        public short Systems { get; set; }
    }
}