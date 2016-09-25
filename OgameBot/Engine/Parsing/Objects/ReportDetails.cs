using System;

namespace OgameBot.Engine.Parsing.Objects
{
    [Flags]
    public enum ReportDetails
    {
        None = 0,
        Resources = 1,
        Ships = 2,
        Defense = 4,
        Buildings = 8,
        Research = 16,
        All = Resources | Ships | Defense | Buildings | Research
    }
}