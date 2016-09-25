using System;
using OgameBot.Objects;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
{
    public abstract class MessageBase : DataObject
    {
        public MessageTabType TabType { get; set; }

        public int MessageId { get; set; }

        public DateTimeOffset Sent { get; set; }
    }
}