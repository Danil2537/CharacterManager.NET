using CharacterManager.Commands;
using CharacterManager.Database;
using CharacterManager.Models;
using CharacterManager.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CharacterManager.ViewModels
{

    public class CreationVM : ViewModelBase
    {
        //public CharacterManagerDbContextFactory _contextFactory;
        private Class _chosenClass;
        private Species _chosenSpecies;
        private List<Item> _startingEquipment;
        private Class _clickedClass;
        private Models.Species _clickedSpecies;
        private int _strength;
        private int _dexterity;
        private int _constitution;
        private int _intelligence;
        private int _wisdom;
        private int _charisma;
        private Background _chosenBackground;
        private Character _createdCharacter;
        public bool CanCreateCharacter;
        public string CharacterName { get; set; }
        public Skill ClassSkillProf1 { get; set; }
        public Skill ClassSkillProf2 { get; set; }

        public ObservableCollection<Class> AvailableClasses { get; set; }
        public ObservableCollection<Species> AvailableSpecies { get; set; }
        public CommandBase ChooseClassCommand { get; }
        public CommandBase ChooseSpeciesCommand { get; }
        public CommandBase RollAbilityCommand { get; }
        public CommandBase SaveCustomBackgroundCommand { get; }
        public CommandBase SaveExistingBackgroundCommand { get; }

        public Class ChosenClass
        {
            get { return _chosenClass; }
            set
            {
                _chosenClass = value;
                OnPropertyChanged(nameof(ChosenClass));
            }
        }
        public Species ChosenSpecies
        {
            get { return _chosenSpecies; }
            set
            {
                _chosenSpecies = value;
                OnPropertyChanged(nameof(ChosenSpecies));
            }
        }
        public Background ChosenBackground
        {
            get { return _chosenBackground; }
            set
            {
                _chosenBackground = value;
                OnPropertyChanged(nameof(ChosenBackground));
            }
        }
        public Class ClickedClass
        {
            get { return _clickedClass; }
            set
            {
                _clickedClass = value;
                OnPropertyChanged(nameof(ClickedClass));
            }
        }
        public Species ClickedSpecies
        {
            get { return _clickedSpecies; }
            set
            {
                _clickedSpecies = value;
                OnPropertyChanged(nameof(ClickedSpecies));
            }
        }
        private Background _clickedBackground;
        public Background ClickedBackground
        {
            get { return _clickedBackground; }
            set
            {
                _clickedBackground = value;
                OnPropertyChanged(nameof(ClickedBackground));
            }
        }

        public int Strength
        {
            get { return _strength; }
            set
            {
                _strength = value;
                OnPropertyChanged(nameof(Strength));
            }
        }
        public int Dexterity
        {
            get { return _dexterity; }
            set
            {
                _dexterity = value;
                OnPropertyChanged(nameof(Dexterity));
            }
        }
        public int Constitution
        {
            get { return _constitution; }
            set
            {
                _constitution = value;
                OnPropertyChanged(nameof(Constitution));
            }
        }
        public int Intelligence
        {
            get { return _intelligence; }
            set
            {
                _intelligence = value;
                OnPropertyChanged(nameof(Intelligence));
            }
        }
        public int Wisdom
        {
            get { return _wisdom; }
            set
            {
                _wisdom = value;
                OnPropertyChanged(nameof(Wisdom));
            }
        }
        public int Charisma
        {
            get { return _charisma; }
            set
            {
                _charisma = value;
                OnPropertyChanged(nameof(Charisma));
            }
        }

        public Character CreatedCharacter
        {
            get { return _createdCharacter; }
            set
            {
                _createdCharacter = value;
                OnPropertyChanged(nameof(CreatedCharacter));
            }
        }

        private Background _customBackground;
        public Background CustomBackground
        {
            get { return _customBackground; }
            set
            {
                _customBackground = value;
                OnPropertyChanged(nameof(CustomBackground));
            }
        }
        public ObservableCollection<Background> AvailableBackgrounds { get; set; }
        public ObservableCollection<Item> AllItems { get; set; }
        private Item _selectedAvailableItem;
        public Item SelectedAvailableItem
        {
            get { return _selectedAvailableItem; }
            set
            {
                _selectedAvailableItem = value;
                OnPropertyChanged(nameof(SelectedAvailableItem));
            }
        }
        public ObservableCollection<Item> SelectedItems { get; set; }
        public Item SelectedSelectedItem { get; set; }
        public ObservableCollection<Trait> AllTraits { get; set; }
        public Trait SelectedBackgroundTrait
        { 
            get { return SelectedBackgroundTrait; } 
            set { SelectedBackgroundTrait = value; OnPropertyChanged(nameof(SelectedBackgroundTrait)); } 
        }

        public ICommand MoveToSelectedCommand { get; }
        public ICommand MoveToAvailableCommand { get; }
        public ICommand CreateCharacterCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RemoveFromSelectedCommand { get; }
        public ICommand ChooseBackgroundCommand { get; }

        public void Initialize()
        {
            try
            {
                AvailableClasses = new ObservableCollection<Class>(DbManager.GetAllClassesQuery());
                AvailableSpecies = new ObservableCollection<Species>(DbManager.GetAllSpeciesQuery());
                AllItems = new ObservableCollection<Item>(DbManager.GetAllItemsQuery());
                AllTraits = new ObservableCollection<Trait>(DbManager.GetAllTraitsQuery());
                AvailableBackgrounds = new ObservableCollection<Background>(DbManager.GetAllBackgroundsQuery());

                Console.WriteLine("Classes loaded: " + AvailableClasses.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading classes: " + ex.Message);
            }
        }

        public static CreationVM Create(Services.NavigationService navigationService, User user)
        {
                  var viewModel =  CreationVM.Create(navigationService, user);
                  viewModel.Initialize();
                  return viewModel;
        }

        public CreationVM(Services.NavigationService navigationService, User user)
        {
            // Збереження переданого контексту бази даних
            //_contextFactory = contextFactory;

            // Ініціалізація колекцій до завантаження даних
            AvailableClasses = new ObservableCollection<Class>();
            AvailableSpecies = new ObservableCollection<Species>();
            AvailableBackgrounds = new ObservableCollection<Background>();
            // Ініціалізація команд для різних дій
            ChooseClassCommand = new ActionCommand(ChooseClass);
            ChooseSpeciesCommand = new ActionCommand(ChooseSpecies);
            ChooseBackgroundCommand = new ActionCommand(ChooseBackground);
            RollAbilityCommand = new ActionCommand(RollAbility);

            CustomBackground = new Background();
            MoveToSelectedCommand = new ActionCommand(MoveToSelected);
            SaveCustomBackgroundCommand = new ActionCommand(SaveCustomBackground);
            SaveExistingBackgroundCommand = new ActionCommand(SaveExistingBackground);

            SelectedItems = new ObservableCollection<Item>();
            RemoveFromSelectedCommand = new ActionCommand(RemoveFromSelected);
            CreateCharacterCommand = new CreateCharacterCommand(this, user, navigationService);
            CancelCommand = new NavigateCommand(navigationService);
            Initialize();
        }


        public CreationVM(ObservableCollection<Species> availableSpecies, ObservableCollection<Class> availableClasses)
        {
            AvailableSpecies = availableSpecies;
            AvailableClasses = availableClasses;
            ChooseClassCommand = new ActionCommand(ChooseClass);
            ChooseSpeciesCommand = new ActionCommand(ChooseSpecies);
            RollAbilityCommand = new ActionCommand(RollAbility);
        }
        private void SelectClass(object obj)
        {
            if (obj is Models.Class selectedClass)
            {
                ClickedClass = selectedClass;
            }
        }
        private void ChooseClass(object obj)
        {
            if (obj is Models.Class chosenClass)
            {
                ChosenClass = chosenClass;
            }
        }
        private void ChooseSpecies(object obj)
        {
            if (obj is Models.Species chosenSpecies)
            {
                ChosenSpecies = chosenSpecies;
            }
        }

        private void ChooseBackground(object obj)
        {
            if (obj is Models.Background chosenBackground)
            {
                ChosenBackground = chosenBackground;
            }
        }
        private void RollAbility(object obj)
        {
            if (obj is string abilityName)
            {
                //Dice abilityDice = new Dice(DiceType.D20,1);
                int abilityScore = Dice.RollAbility();

                switch (abilityName)
                {
                    case "Strength":
                        Strength = abilityScore;
                        break;
                    case "Dexterity":
                        Dexterity = abilityScore;
                        break;
                    case "Constitution":
                        Constitution = abilityScore;
                        break;
                    case "Intelligence":
                        Intelligence = abilityScore;
                        break;
                    case "Wisdom":
                        Wisdom = abilityScore;
                        break;
                    case "Charisma":
                        Charisma = abilityScore;
                        break;
                    default:
                        throw new ArgumentException("Invalid ability name", nameof(abilityName));
                }
            }
        }
        private void MoveToSelected(object obj)
        {

            CustomBackground.AddBackgroundItem(SelectedAvailableItem);
            SelectedItems.Add(SelectedAvailableItem);
            AllItems.Remove(SelectedAvailableItem);
        }
        private void RemoveFromSelected(object obj)
        {
            if (SelectedSelectedItem != null)
            {
                var item = SelectedSelectedItem;
                CustomBackground.RemoveBackgroundItem(SelectedSelectedItem);
                SelectedItems.Remove(SelectedSelectedItem);
                AllItems.Add(item);
            }
        }
        private void SaveExistingBackground(object obj)
        {
            ChooseBackground(obj);

        }
        private void SaveCustomBackground(object obj)
        {
            ChosenBackground = CustomBackground;
        }
    }
}
