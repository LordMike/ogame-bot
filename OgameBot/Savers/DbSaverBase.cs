using System.Collections.Generic;
using ScraperClientLib.Engine;

namespace OgameBot.Savers
{
    public abstract class DbSaverBase
    {
        public abstract void Run(List<DataObject> result);
    }
}