using System;

namespace OgameBot.Db.Interfaces
{
    public interface IModifiedOn
    {
        DateTimeOffset UpdatedOn { get; set; }
    }
}