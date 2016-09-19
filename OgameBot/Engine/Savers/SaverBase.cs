using System.Collections.Generic;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Savers
{
    public abstract class SaverBase
    {
        public abstract void Run(List<DataObject> result);
    }
}