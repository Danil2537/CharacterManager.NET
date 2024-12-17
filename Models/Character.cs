using CharacterManager.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManager.Models
{
    public class AbilityScore
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public Ability Ability { get; set; }
        public int Score { get; set; }
    }
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Level { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;

        [Required]
        public int SpeciesId { get; set; }
        public Species Species { get; set; } = null!;

        [Required]
        public int BackgroundId { get; set; }
        public Background Background { get; set; } = null!;

        public ICollection<Spell> Spells { get; set; } = new List<Spell>();
        public ICollection<Trait> Traits { get; set; } = new List<Trait>();

        // Change this property to be a collection of AbilityScore entities
        public ICollection<AbilityScore> AbilityScores { get; set; } = new List<AbilityScore>();

        public List<Skill> Skills { get; set; } = new List<Skill>();
        

        public List<Language> Languages { get; set; } = new List<Language>();

        public List<Tool> Tools { get; set; } = new List<Tool>();
        private int proficiencyBonus;
        public int ProficiencyBonus { get => proficiencyBonus; set => proficiencyBonus = (7 + Level) / 4; }
        public List<Skill> ProficientSkills { get; set; } = new List<Skill>();
        public int ArmorClass { get; set; }
        public int InitiativeBonus { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        [NotMapped]
        public ObservableCollection<Tuple<bool, Ability, int>> SaveValues { get; set; } = new ObservableCollection<Tuple<bool, Ability, int>>();

        [NotMapped]
        public ObservableCollection<Tuple<bool,Skill,int>> SkillValues { get; set; } = new ObservableCollection<Tuple<bool, Skill, int>>();
        //public int? HitDieId { get; set; }

        //[ForeignKey("HitDieId")] 
        //public Dice? HitDie { get; set; }
        public int HitDieNumber { get; set; } = 1;
        public ICollection<CharacterItem> CharacterItems { get; set; } = new List<CharacterItem>();

        public Character()
        {
            Name = string.Empty;
            Level = 1;
            UserId = 0;
            ClassId = 0;
            SpeciesId = 0;
            BackgroundId = 0;
            HitDieNumber = 1;

        }
        public Character(string characterName, Class chosenClass, Species chosenSpecies, Background chosenBack, int level)
        {
            Name = characterName;
            Class = chosenClass;
            ClassId = chosenClass.Id;
            Species = chosenSpecies;
            SpeciesId = chosenSpecies.Id;
            Background = chosenBack;
            BackgroundId = chosenBack.Id;
            Level = level;
            proficiencyBonus = (7 + Level) / 4;
            ProficiencyBonus = (7+Level) / 4;
            // Assign items from background
            foreach (var bitem in chosenBack.BackgroundItems)
            {
                CharacterItems.Add(new CharacterItem { Character = this, Item = bitem.Item });
            }

            // Assign items from class
            foreach (var citem in chosenClass.ClassItems)
            {
                var charItem = new CharacterItem { Character = this, Item = citem.Item };
                if (!CharacterItems.Contains(charItem)) 
                {
                    CharacterItems.Add(charItem);
                }
            }
            Class.Proficiencies = DbManager.GetClassProfsQuery(Class.Id);
            
        }

        public void SetAbilities(int str, int dex, int con, int intelect, int wis, int cha)
        {
            var newAbilityScores = new List<AbilityScore>
            {
                new AbilityScore { Ability = Ability.Strength, Score = str },
                new AbilityScore { Ability = Ability.Dexterity, Score = dex },
                new AbilityScore { Ability = Ability.Constitution, Score = con },
                new AbilityScore { Ability = Ability.Intelligence, Score = intelect },
                new AbilityScore { Ability = Ability.Wisdom, Score = wis },
                new AbilityScore { Ability = Ability.Charisma, Score = cha }
            };

            // Update the AbilityScores collection
            AbilityScores.Clear(); // Clear existing scores
            foreach (var abilityScore in newAbilityScores)
            {
                AbilityScores.Add(abilityScore);
            }
            SetSaves();
        }
        public int GetAbility(Ability ability)
        {
            // Find the ability score for the specified ability
            var abilityScore = AbilityScores.FirstOrDefault(a => a.Ability == ability)?.Score;

            // Return the score if found, otherwise return a default of 10
            return abilityScore ?? 10;
        }
        public int GetModifier(int abilityValue)
        {
            return (abilityValue - 10) / 2;
        }
        public void SetSkills()
        {
            var abilityToSkillsMap = new Dictionary<Ability, List<Skill>>()
            {
                { Ability.Strength, new List<Skill> { Skill.Athletics } },
                { Ability.Dexterity, new List<Skill> { Skill.Acrobatics, Skill.SleightOfHand, Skill.Stealth } },
                { Ability.Intelligence, new List<Skill> { Skill.Arcana, Skill.History, Skill.Investigation, Skill.Nature, Skill.Religion } },
                { Ability.Wisdom, new List<Skill> { Skill.AnimalHandling, Skill.Insight, Skill.Medicine, Skill.Perception, Skill.Survival } },
                { Ability.Charisma, new List<Skill> { Skill.Deception, Skill.Intimidation, Skill.Performance, Skill.Persuasion } }
            };

            foreach (var entry in abilityToSkillsMap)
            {
                var abilityScore = AbilityScores.FirstOrDefault(a => a.Ability == entry.Key)?.Score ?? 10;

                int modifier = GetModifier(abilityScore);

                foreach (var skill in entry.Value)
                {
                    if (Skills.Count < 18)
                    {
                        Skills.Add(skill);
                    }

                    int skillModifier = modifier;
                    if (ProficientSkills.Contains(skill))
                    {
                        skillModifier += (7 + Level) / 4;
                    }
                    var skillValue = new Tuple<bool, Skill, int>(ProficientSkills.Contains(skill), skill, skillModifier);
                    this.SkillValues.Add(skillValue);

                }
            }
        }
        public void AddSkillProficiency(Skill skill)
        {
            if (!ProficientSkills.Contains(skill))
            {
                ProficientSkills.Add(skill);
            }
        }
        public int GetSavingThrow(Ability ability)
        {
            int score = 0;
            foreach (AbilityScore abs in this.AbilityScores)
            {
                if (abs.Ability == ability) { score = abs.Score; break; }
            }
            return (this.Class.ProficientAbility1 == ability || this.Class.ProficientAbility2 == ability) ? GetModifier(score)+this.ProficiencyBonus: GetModifier(score);
        }
        public void SetSaves()
        {
            SaveValues.Clear();
            foreach (var abs in AbilityScores)
            {
                int saveValue = GetModifier(abs.Score);
                bool isProficient = Class.ProficientAbility1 == abs.Ability || Class.ProficientAbility2 == abs.Ability;
                if (isProficient)
                {
                    if (this.ProficiencyBonus == 0) { this.ProficiencyBonus = (7 + this.Level) / 4; }
                     saveValue += this.ProficiencyBonus;
                }
                SaveValues.Add(new Tuple<bool, Ability, int>(isProficient, abs.Ability, saveValue));
            }
        }

    }

    public class CharacterItem
    {
        public int CharacterId { get; set; }
        public Character Character { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
    public enum Ability
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }

    public enum Skill
    {
        Acrobatics,
        AnimalHandling,
        Arcana,
        Athletics,
        Deception,
        History,
        Insight,
        Intimidation,
        Investigation,
        Medicine,
        Nature,
        Perception,
        Performance,
        Persuasion,
        Religion,
        SleightOfHand,
        Stealth,
        Survival
    }

    public enum Language
    {
        Common,
        Dwarvish,
        Elvish,
        Gnomish,
        Goblin,
        Halfling,
        Orc,
        Draconic,
        Sylvan,
        Undercommon
    }

    public enum Tool
    {
        AlchemistSupplies,
        BrewerSupplies,
        CalligrapherSupplies,
        CarpenterTools,
        CartographerTools,
        CobblerTools,
        CookUtensils,
        GlassblowerTools,
        JewelerTools,
        LeatherworkerTools,
        MasonTools,
        PainterTools,
        PotterTools,
        SmithTools,
        TinkerTools,
        WeaverTools,
        WoodcarverTools
    }
}
