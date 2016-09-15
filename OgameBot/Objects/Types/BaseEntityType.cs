using System.Collections.Generic;

namespace OgameBot.Objects.Types
{
    public abstract class BaseEntityType<TType, TValue> where TType : struct where TValue : BaseEntityType<TType, TValue>
    {
        protected static readonly Dictionary<TType, TValue> Index;

        public TType Type { get; }

        static BaseEntityType()
        {
            Index = new Dictionary<TType, TValue>();
        }

        protected BaseEntityType(TType type)
        {
            Type = type;

            Index[type] = this as TValue;
        }

        public static TValue Get(TType type)
        {
            return Index[type];
        }
    }
}