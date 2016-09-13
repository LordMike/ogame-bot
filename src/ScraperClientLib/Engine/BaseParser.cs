using System.Collections.Generic;

namespace ScraperClientLib.Engine
{
    public abstract class BaseParser
    {
        protected bool ProcessOnlySuccesses { get; set; } = true;

        protected BaseParser()
        {

        }

        public bool ShouldProcess(ResponseContainer container)
        {
            if (ProcessOnlySuccesses && !container.WasSuccess)
                return false;

            return ShouldProcessInternal(container);
        }

        public IEnumerable<DataObject> Process(ClientBase client, ResponseContainer container)
        {
            string typeName = GetType().FullName;
            foreach (DataObject dataObject in ProcessInternal(client, container))
            {
                dataObject.ParserType = typeName;
                yield return dataObject;
            }
        }

        public abstract bool ShouldProcessInternal(ResponseContainer container);

        public abstract IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container);
    }
}