using System;

namespace OgameBot.Db.Interfaces
{
    public interface ICreatedOn
    {
        DateTimeOffset CreatedOn { get; set; }
    }
}