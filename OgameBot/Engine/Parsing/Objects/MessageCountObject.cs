using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
{
    public class MessageCountObject : DataObject
    {
        public int NewMessages { get; set; }

        public override string ToString()
        {
            return $"New messages: {NewMessages:N0}";
        }
    }
}