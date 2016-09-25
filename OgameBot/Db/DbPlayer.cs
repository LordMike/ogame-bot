using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OgameBot.Db.Interfaces;
using OgameBot.Engine.Parsing.Objects;

namespace OgameBot.Db
{
    public class DbPlayer : ICreatedOn, IModifiedOn
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PlayerId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }
        
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    
        public virtual ICollection<DbPlanet> Planets { get; set; }

        public PlayerStatus Status { get; set; }

        public override string ToString()
        {
            return $"{PlayerId} ({Name})";
        }
    }
}