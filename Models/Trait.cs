using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManager.Models
{
    public enum TraitType { action, bonus_action, reaction, other };

    [Table("Traits")]
    public class Trait
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TraitType Type { get; set; }
        public int UnlockAtLevel { get; set; }

        // Foreign key for Class
        public int? ClassId { get; set; }
        public Class? Class { get; set; }

        // Foreign key for Species
        public int? SpeciesId { get; set; }
        public Species? Species { get; set; }

        // Foreign key for Character
        public int? CharacterId { get; set; }
        public Character? Character { get; set; }

        public Trait()
        {
            Name = string.Empty;
            Description = string.Empty;
            Type = TraitType.action;
            UnlockAtLevel = 1;
        }

        public Trait(string name, string description, TraitType traitType, int unlockAt)
        {
            Name = name;
            Description = description;
            Type = traitType;
            UnlockAtLevel = unlockAt;
        }
    }

}
