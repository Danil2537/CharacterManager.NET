using System.Collections.ObjectModel;
using CharacterManager.Models;
using CharacterManager.Commands;
using System.Windows.Input;

namespace CharacterManager.ViewModels
{
    public class CreateClassVM : ViewModelBase
    {
        // Properties for binding
        private string _className;
        private string _classDescription;
        private Ability _profSave1;
        private Ability _profSave2;
        private DiceType _hitDie;
        private bool _hasSpells;
        private Ability _spellAbility;
        private ObservableCollection<Proficiency> _proficiencies;
        private ObservableCollection<Skill> _skillProfs;
        private ObservableCollection<Trait> _classAbilities;
        private int _profSkillNumber;
        private DiceType _goldDieType;
        private int _goldDieNumber;

        public ICommand EnterProfileCommand { get; }

        public CreateClassVM(Services.NavigationService profileNavigationService)
        {
            EnterProfileCommand = new NavigateCommand(profileNavigationService);
            Proficiencies = new ObservableCollection<Proficiency>();
            SkillProfs = new ObservableCollection<Skill>();
            ClassAbilities = new ObservableCollection<Trait>();

            // Initialize commands
            AddProficiencyCommand = new ActionCommand(AddProficiency);
            RemoveProficiencyCommand = new ActionCommand(RemoveProficiency);
            AddTraitCommand = new ActionCommand(AddTrait);
            RemoveTraitCommand = new ActionCommand(RemoveTrait);
        }
        public string ClassName
        {
            get => _className;
            set { _className = value; OnPropertyChanged(nameof(ClassName)); }
        }

        public string ClassDescription
        {
            get => _classDescription;
            set { _classDescription = value; OnPropertyChanged(nameof(ClassDescription)); }
        }

        public Ability ProfSave1
        {
            get => _profSave1;
            set { _profSave1 = value; OnPropertyChanged(nameof(ProfSave1)); }
        }

        public Ability ProfSave2
        {
            get => _profSave2;
            set { _profSave2 = value; OnPropertyChanged(nameof(ProfSave2)); }
        }

        public DiceType HitDie
        {
            get => _hitDie;
            set { _hitDie = value; OnPropertyChanged(nameof(HitDie)); }
        }

        public bool HasSpells
        {
            get => _hasSpells;
            set { _hasSpells = value; OnPropertyChanged(nameof(HasSpells)); }
        }

        public Ability SpellAbility
        {
            get => _spellAbility;
            set { _spellAbility = value; OnPropertyChanged(nameof(SpellAbility)); }
        }

        public ObservableCollection<Proficiency> Proficiencies
        {
            get => _proficiencies;
            set { _proficiencies = value; OnPropertyChanged(nameof(Proficiencies)); }
        }

        public ObservableCollection<Skill> SkillProfs
        {
            get => _skillProfs;
            set { _skillProfs = value; OnPropertyChanged(nameof(SkillProfs)); }
        }

        public ObservableCollection<Trait> ClassAbilities
        {
            get => _classAbilities;
            set { _classAbilities = value; OnPropertyChanged(nameof(ClassAbilities)); }
        }


        public int ProfSkillNumber
        {
            get => _profSkillNumber;
            set { _profSkillNumber = value; OnPropertyChanged(nameof(ProfSkillNumber)); }
        }

        public DiceType GoldDieType
        {
            get => _goldDieType;
            set { _goldDieType = value; OnPropertyChanged(nameof(GoldDieType)); }
        }

        public int GoldDieNumber
        {
            get => _goldDieNumber;
            set { _goldDieNumber = value; OnPropertyChanged(nameof(GoldDieNumber)); }
        }

        // Commands
        public ICommand AddProficiencyCommand { get; }
        public ICommand RemoveProficiencyCommand { get; }
        public ICommand AddTraitCommand { get; }
        public ICommand RemoveTraitCommand { get; }

  

        // Command methods
        private void AddProficiency(object obj)
        {
            Proficiencies.Add(new Proficiency());
        }

        private void RemoveProficiency(object proficiency)
        {
            if (proficiency is Proficiency prof)
            {
                Proficiencies.Remove(prof);
            }
        }

        private void AddTrait(object obj)
        {
            ClassAbilities.Add(new Trait());
        }

        private void RemoveTrait(object trait)
        {
            if (trait is Trait Trait)
            {
                ClassAbilities.Remove(Trait);
            }
        }
    }
}
