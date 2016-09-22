using System;

namespace OgameBot.Engine.Parsing.Objects
{
    [Flags]
    public enum PlayerStatus
    {
        None = 0,
        Noob = 1 << 0,
        Vacation = 1 << 1,
        Strong = 1 << 2,
        Banned = 1 << 3,
        Active = 1 << 4,
        Inactive = 1 << 5,
        LongInactive = 1 << 6,
        Outlaw = 1 << 7,
        HonorableTarget = 1 << 8,
        AllyOwn = 1 << 9,
        AllyWar = 1 << 10,
        Buddy = 1 << 11,
        Admin = 1 << 12
    }
}