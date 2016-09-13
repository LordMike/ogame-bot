using System.Collections.Generic;

namespace ScraperClientLib.Engine
{
    public abstract class BaseParser
    {
        protected bool ProcessOnlySuccesses { get; set; } = true;

        protected BaseParser()
        {

        }

        public bool ShouldProcess(ResponseDocument document)
        {
            if (ProcessOnlySuccesses && !document.WasSuccess)
                return false;

            return ShouldProcessInternal(document);
        }

        public IEnumerable<DataObject> Process(ClientBase client, ResponseDocument document)
        {
            string typeName = GetType().FullName;
            foreach (DataObject dataObject in ProcessInternal(client, document))
            {
                dataObject.ParserType = typeName;
                yield return dataObject;
            }
        }

        public abstract bool ShouldProcessInternal(ResponseDocument document);

        public abstract IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseDocument document);
    }
}