using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OgameBot.Db.Interfaces;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Objects;
using OgameBot.Utilities;

namespace OgameBot.Db
{
    public class DbMessage : ILazySaver, ICreatedOn
    {
        private MessageBase _message;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MessageId { get; set; }

        public MessageTabType TabType { get; set; }

        /// <summary>
        /// Do not mess with this
        /// </summary>
        [Required]
        [MaxLength(4096)]
        public byte[] SerializedMessage { get; set; }

        [Required]
        [MaxLength(128)]
        public string SerializedMessageType { get; set; }

        [NotMapped]
        public MessageBase Message
        {
            get
            {
                if (_message == null)
                {
                    Type type = Type.GetType(SerializedMessageType, true);
                    _message = (MessageBase)SerializerHelper.DeserializeFromBytes(type, SerializedMessage, true);
                }

                return _message;
            }
            set { _message = value; }
        }

        public DateTimeOffset CreatedOn { get; set; }

        public void Update()
        {
            SerializedMessage = SerializerHelper.SerializeToBytes(Message, true);
            SerializedMessageType = Message.GetType().FullName;
        }
    }
}