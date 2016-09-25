using System.Collections.Generic;
using System.Linq;
using OgameBot.Db;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Utilities;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Savers
{
    public class MessageSaver : SaverBase
    {
        public override void Run(List<DataObject> result)
        {
            List<MessageBase> messages = result.OfType<MessageBase>().ToList();
            if (!messages.Any())
                return;

            int[] messageIds = messages.Select(s => s.MessageId).ToArray();

            using (BotDb db = new BotDb())
            {
                HashSet<int> existing = db.Messages.Where(s => messageIds.Contains(s.MessageId)).Select(s => s.MessageId).ToHashset();

                foreach (MessageBase message in messages.Where(s => !existing.Contains(s.MessageId)))
                {
                    db.Messages.Add(new DbMessage
                    {
                        MessageId = message.MessageId,
                        Message = message,
                        TabType = message.TabType
                    });
                }

                db.SaveChanges();
            }
        }
    }
}