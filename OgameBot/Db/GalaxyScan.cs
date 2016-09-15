using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OgameBot.Db
{
    public class GalaxyScan
    {
        [Key, Column(Order = 0)]
        public byte Galaxy { get; set; }

        [Key, Column(Order = 1)]
        public short System { get; set; }

        public DateTime LastScan { get; set; }
    }
}