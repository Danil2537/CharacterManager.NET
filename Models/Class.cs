using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace CharacterManager.Models
{

    [Table("Classes")]
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? HitDiceId { get; set; }
        [ForeignKey("HitDiceId")]
        public Dice? HitDice { get; set; }

        public Ability ProficientAbility1 { get; set; }
        public Ability ProficientAbility2 { get; set; }
        public List<Skill> ProficientSkillOptions { get; set; }

        public int StartingGold { get; set; }
        public bool HasSpells { get; set; }
       
        public Ability SpellCastingAbility { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }

        public int? StartingGoldDiceId { get; set; }
        [ForeignKey("StartingGoldDiceId")]
        public Dice? StartingGoldDice { get; set; }

        public ICollection<Trait> Traits { get; set; } = new List<Trait>();

        public List<ClassItem> ClassItems { get; set; } = new List<ClassItem>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<ClassSpell> ClassSpells { get; set; } = new List<ClassSpell>();
        public List<Spell> Spells { get; set; } = new List<Spell>();
        public List<ClassProficiency> ClassProficiencies { get; set; } = new List<ClassProficiency>();
        public List<Proficiency> Proficiencies { get; set; } = new List<Proficiency>();

        public Class()
        {
            Proficiencies = new List<Proficiency>();
            Spells = new List<Spell>();
            Items = new List<Item>();
            ProficientSkillOptions = new List<Skill>();
            Name = "DefaultClassName";
            Description = "DefaultClassDescription";
            UserId = 0;
            StartingGoldDice = new Dice(DiceType.D4, 4);
            StartingGold = StartingGoldDice.Roll() * 10;
        }
    }

    public enum ProficiencyType
    {
        Armor,
        Weapon,
        Tool
    }

    public class Proficiency
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ProficiencyType Type { get; set; } 
        public string Name { get; set; }

        public List<ClassProficiency>? ClassProficiencies { get; } = new List<ClassProficiency>();
        public List<Class>? Classes { get; } = new List<Class>();
        public Proficiency()
        {
            Type = ProficiencyType.Armor;
            Name = string.Empty;
            //ClassId = null;
        }
    }

    public class ClassProficiency
    {
        public int ClassesId { get; set; }
        public int ProficienciesId { get; set; }
        public Class Class { get; set; } = null!;
        public Proficiency Proficiency { get; set; } = null!;
    }

    public class ClassItem
    {
        public int ClassesId { get; set; }
        public int ItemsId { get; set; }
        public Class Class { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }

    public class ClassSpell
    {
        public int ClassesId { get; set; }
        public int SpellsId { get; set; }
        public Class Class { get; set; } = null!;
        public Spell Spell { get; set; } = null!;
    }
}
