using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CharacterManager.Models
{
    [Table("Items")]
    public class Item
    {
        // Primary key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Name and Description of the item
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Boolean flags for item types (e.g., Armor, Weapon)
        public bool IsArmor { get; set; }
        public bool IsWeapon { get; set; }
        public bool? BelongToBackground { get; set; }
        public bool? BelongToClass { get; set; }
        //public int ClassId { get; set; }
        //public Class Class { get; set; } = null!;


        public List<BackgroundItem>? BackgroundItems { get; } = new List<BackgroundItem>();
        public List<Background>? Backgrounds { get; } = new List<Background>();

        public List<ClassItem>? ClassItems { get; } = new List<ClassItem>();
        public List<Class>? Classes { get; } = new List<Class>();
        public ICollection<CharacterItem> CharacterItems { get; set; } = new List<CharacterItem>();

        public Item()
        {
            // Default values can be set here
            Name = string.Empty;
            Description = string.Empty;
            IsArmor = false;
            IsWeapon = false;
        }

        // Parameterized constructor for convenience
        public Item(string name, string description, bool isArmor = false, bool isWeapon = false)
        {
            Name = name;
            Description = description;
            IsArmor = isArmor;
            IsWeapon = isWeapon;
        }
    }
}
