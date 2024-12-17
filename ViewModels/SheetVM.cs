using CharacterManager.Commands;
using CharacterManager.Database;
using CharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CharacterManager.ViewModels
{
    public class SheetVM : ViewModelBase
    {
        public ICommand navigateProfileCommand { get; }
        public ICommand navigateLvlupCommand { get; }
        private Character _character;
        public Character Character
        {
            get { return _character; }
            set
            {
                _character = value;
                _character.ProficiencyBonus = (7 + _character.Level) / 4;
                OnPropertyChanged(nameof(Character));
            }
        }
        private Class _class;
        public Class Class
        {
            get { return _class; }
            set
            {
                _class = value;
                OnPropertyChanged(nameof(Class));
            }
        }
        private Species _species;
        public Species Species
        {
            get { return _species; }
            set
            {
                _species = value;
                OnPropertyChanged(nameof(Species));
            }
        }
        private Background _background;
        public Background Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }
        //public ObservableCollection<Item> Weapons { get; set; }
        //public ObservableCollection<Item> Armor {get;set;}
        //public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Tuple<Ability, int, int>> Abilities { get; set; } = new ObservableCollection<Tuple<Ability, int, int>>();
        public Dice SheetDice { get; set; } = new Dice(DiceType.D20,1);
        public CommandBase RollCommand { get; set; }
        public string _diceResult;
        public string DiceResult
        {
            get { return _diceResult; }
            set
            {
                _diceResult = value;
                OnPropertyChanged(nameof(DiceResult));
            }
        }
        public int _tempHealth;
        public int TempHealth
        {
            get { return _tempHealth; }
            set
            {
                _tempHealth = value;
                OnPropertyChanged(nameof(TempHealth));
            }
        }
        public string Resistances { get; set; }
        private ObservableCollection<Proficiency> _proficiencies;
        public ObservableCollection<Proficiency> Proficiencies
        {
            get { return _proficiencies; }
            set
            {
                _proficiencies = value;
                OnPropertyChanged(nameof(Proficiencies));
            }
        }
        private ObservableCollection<Item> _inventory;
        public ObservableCollection<Item> Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }
        public ObservableCollection<Item> Weapons { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Trait> Actions { get; set; } = new ObservableCollection<Trait>();
        public ObservableCollection<Trait> BonusActions { get; set; } = new ObservableCollection<Trait>();
        public ObservableCollection<Trait> Reactions { get; set; } = new ObservableCollection<Trait>();
        public ObservableCollection<Trait> OtherTraits { get; set; } = new ObservableCollection<Trait>();

        //public ObservableCollection<Tuple<Skill, int>> Skills { get; set; } = new ObservableCollection<Tuple<Skill, int>>();
        public SheetVM(Services.NavigationService profileNavigationService, Services.NavigationService lvlupNavigationService, Character character)
        {
            navigateProfileCommand = new NavigateCommand(profileNavigationService, character.User);
            navigateLvlupCommand = new NavigateCommand(lvlupNavigationService);
            RollCommand = new ActionCommand(Roll);
            _character = character;
            _character.ProficiencyBonus = (7 + _character.Level) / 2;
            if (_character.SkillValues.Count==0) { _character.SetSkills(); }
            if(_character.SaveValues.Count==0) { _character.SetSaves(); }
            
            _class = character.Class;
            _species = character.Species;
            _background = character.Background;
            _diceResult = "Dice Roll Result:";
            foreach(AbilityScore abs in character.AbilityScores)
            {
                Abilities.Add(new Tuple<Ability, int, int>(abs.Ability, abs.Score, character.GetModifier(abs.Score)));
            }
            if (_species.DamageResistances != null)
            {
                foreach (string res in _species.DamageResistances)
                {
                    Resistances += res + " ";
                }
            }
            else { Resistances = "No Resistances"; }
            
            _proficiencies =new ObservableCollection<Proficiency>(DbManager.GetClassProfsQuery(_character.Class.Id));
            _inventory = new ObservableCollection<Item>(DbManager.GetCharacterItems(_character.Id));
            foreach(Item it in _inventory)
            {
                if (it.IsWeapon) { Weapons.Add(it); }
            }
            AddTraitsToLists(_character.Class.Traits);
            AddTraitsToLists(_character.Species.Traits);
            AddTraitsToLists(_character.Traits);
            AddTraitsToLists(new[] { _character.Background.BackgroundTrait });
        }
        void AddTraitsToLists(IEnumerable<Trait> traits)
        {
            foreach (Trait tr in traits)
            {
                switch (tr.Type)
                {
                    case TraitType.action:
                        Actions.Add(tr);
                        break;
                    case TraitType.bonus_action:
                        BonusActions.Add(tr);
                        break;
                    case TraitType.reaction:
                        Reactions.Add(tr);
                        break;
                    case TraitType.other:
                        OtherTraits.Add(tr);
                        break;
                }
            }
        }
        private void Roll(object obj)
        {
            int diceNumber = SheetDice.Roll();
            DiceResult = $"Dice Roll Result: \n {diceNumber} + {(int)obj} = {diceNumber+(int)obj}";
        }
    }
}
