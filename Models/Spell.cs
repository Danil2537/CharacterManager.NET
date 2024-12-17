using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManager.Models
{
    [Table("Spells")]
    public class Spell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public SpellSchool School { get; set; }

        [Required]
        public string CastingTime { get; set; } = string.Empty;

        public string Range { get; set; } = "Self"; 

        public bool IsConcentration { get; set; }

        public string Components { get; set; } = string.Empty;
        public List<ClassSpell> ClassSpells { get; } = new List<ClassSpell>();
        public List<Class> Classes { get; } = new List<Class>();

        public Spell()
        {
            Name = string.Empty;
            Description = string.Empty;
            School = SpellSchool.Abjuration;  // Default school
            CastingTime = "1 action";  // Default to 1 action
            Range = "Self";  // Default to self
            IsConcentration = false;  // Default to no concentration
            Components = "V";  // Default to verbal component
        }

        public Spell(string name, string description, SpellSchool school, string castingTime, string range = "Self", bool isConcentration = false, string components = "V")
        {
            Name = name;
            Description = description;
            School = school;
            CastingTime = castingTime;
            Range = range;
            IsConcentration = isConcentration;
            Components = components;
        }
    }

    // Enum for Spell Schools for better readability
    public enum SpellSchool
    {
        Abjuration,
        Conjuration,
        Divination,
        Enchantment,
        Evocation,
        Illusion,
        Necromancy,
        Transmutation
    }
}
