using System.Collections.Generic;
using ScraperClientLib.Engine;

namespace OgameBot.Savers
{
    public abstract class SaverBase
    {
        public abstract void Run(List<DataObject> result);
    }
}