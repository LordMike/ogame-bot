using System;
using System.Collections.Generic;
using OgameBot.Objects;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
{
    public class MessagesPage : DataObject
    {
        public MessageTabType TabType { get; set; }

        public int Page { get; set; }

        public int MaxPage { get; set; }

        public List<Tuple<int, MessageType>> MessageIds { get; set; }

        public MessagesPage()
        {
            MessageIds = new List<Tuple<int, MessageType>>();
        }

        public override string ToString()
        {
            return $"Messages {TabType}, page {Page:N0} of {MaxPage:N0} - {MessageIds.Count:N0} items";
        }
    }
}