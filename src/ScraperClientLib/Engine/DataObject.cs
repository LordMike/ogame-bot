namespace ScraperClientLib.Engine
{
    public abstract class DataObject
    {
        public string ParserType { get; internal set; }

        public abstract override string ToString();
    }
}