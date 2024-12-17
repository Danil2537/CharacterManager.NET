using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CharacterManager.Models
{
    [Table("Species")]
    public class Species
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // Size (Enum)
        public Size Size { get; set; }

        // Speed in feet
        public int Speed { get; set; }

        public List<Language> SpeciesLanguages { get; set; } = new List<Language>();

        // Resistances to specific types of damage (e.g., "Fire", "Poison")
        public List<string>? DamageResistances { get; set; } = new List<string>();

        // Darkvision range, 0 if not applicable
        public int? Darkvision { get; set; }

        public ICollection<Trait> Traits { get; set; } = new List<Trait>();

        public Species()
        {
            SpeciesLanguages = new List<Language>();
            //DamageResistances = new List<string>();
            //SpeciesAbilities = new List<Trait>();
        }
    }

    public enum Size
    {
        Tiny,
        Small,
        Medium,
        Large,
        Huge,
        Gargantuan
    }
}
