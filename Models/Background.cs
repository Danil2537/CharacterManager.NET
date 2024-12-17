using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManager.Models
{
    [Table("Backgrounds")]
    public class Background
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // Foreign key references for SkillProfs, using SkillType enum
        public Skill SkillProf1 { get; set; }
        public Skill SkillProf2 { get; set; }

        // List of languages (foreign key to Language table)
        public Language BackgroundLanguage { get; set; }

        public string PersonalityTrait { get; set; }
        public string Ideal { get; set; }
        public string Bond { get; set; }
        public string Flaw { get; set; }

        // Foreign key reference for a single Trait
        public int? BackgroundTraitId { get; set; }
        public Trait BackgroundTrait { get; set; }

        public List<BackgroundItem> BackgroundItems { get; set; } = new List<BackgroundItem>();
        public List<Item> Items { get; set; } = new List<Item>();
        public Background()
        {
            Id = 0;
            Name = string.Empty;
            Description = string.Empty;
            SkillProf1 = Skill.Acrobatics;
            SkillProf2 = Skill.AnimalHandling;
            //BackgroundLanguages = string.Empty;
            PersonalityTrait = string.Empty;
            Ideal = string.Empty;
            Bond = string.Empty;
            Flaw = string.Empty;
            BackgroundTraitId = 0;
        }
        public Background(string name, string description, Skill skillProf1, Skill skillProf2, string backgroundLanguages, List<Item> items, string personalityTrait, string backgroundIdeal, string backgroundBond, string backgroundFlaw)
        {
            Name = name;
            Description = description;
            SkillProf1 = skillProf1;
            SkillProf2 = skillProf2;
            //BackgroundLanguages = backgroundLanguages;
            PersonalityTrait = personalityTrait;
            Ideal = backgroundIdeal;
            Bond = backgroundBond;
            Flaw = backgroundFlaw;
        }
        public void AddBackgroundItem(Item item)
        {
            item.BelongToBackground = true;
            BackgroundItems.Add(new BackgroundItem(this, item));
        }
        public void RemoveBackgroundItem(Item item)
        {
            item.BelongToBackground = false;
            this.BackgroundItems.Remove(new BackgroundItem(this, item));
        }
    }


    public class BackgroundItem
    {
        public int BackgroundId { get; set; }
        public Background Background { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
        public BackgroundItem()
        {
            Background = new Background();
            BackgroundId =0;
            Item = new Item();
            ItemId = 0;
        }
        public BackgroundItem(Background back, Item it)
        {
            Background = back;
            BackgroundId = back.Id;
            Item = it;
            ItemId = it.Id;
        }
    }
}
