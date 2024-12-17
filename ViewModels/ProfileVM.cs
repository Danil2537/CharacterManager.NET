using CharacterManager.Commands;
using CharacterManager.Database;
using CharacterManager.Models;
using CharacterManager.Stores;
using System.Collections.ObjectModel;


namespace CharacterManager.ViewModels
{
    public class ProfileVM : ViewModelBase
    {
        private User _user;
        //private CharacterManagerDbContextFactory _contextFactory;
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string _searchKeyword;
        private string _selectedCategory;
        private ObservableCollection<Character> _filteredCharacters;

        public ObservableCollection<string> SearchCategories { get; } = new ObservableCollection<string>
    {
        "Name",
        "Class",
        "Species",
        "Background"
    };

        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged(nameof(SearchKeyword));
                FilterCharacters(null); // Automatically filter as the keyword changes
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilterCharacters(null); // Refilter if the category changes
            }
        }

        public ObservableCollection<Character> FilteredCharacters
        {
            get => _filteredCharacters;
            set
            {
                _filteredCharacters = value;
                OnPropertyChanged(nameof(FilteredCharacters));
            }
        }

        public CommandBase SearchCommand { get; }

        public void AddCharacter(Character character)
        {
            this._user.AddCharacter(character);
        }
        public void DeleteCharacter(object arg)
        {
            if (arg is Character characterToDelete)
            {
                DbManager.DeleteCharacterQuery(characterToDelete.Id);
                User.CharacterList.Remove(characterToDelete);
                OnPropertyChanged(nameof(User));
            }
        }

        public CommandBase CreateCharacterCommand { get; }
        public CommandBase OpenCharacterCommand { get; }
        public CommandBase DeleteCharacterCommand { get; }
        public CommandBase CreateClassCommand { get; }

        public ProfileVM(Services.NavigationService creationNavigationService, Services.NavigationService sheetNavigationService, Services.NavigationService createClassNavigationService, User user)
        {
            _user = user;
            CreateCharacterCommand = new NavigateCommand(creationNavigationService, _user);
            CreateClassCommand = new NavigateCommand(createClassNavigationService, _user);
            OpenCharacterCommand = new NavigateCommand(sheetNavigationService);
            DeleteCharacterCommand = new ActionCommand(DeleteCharacter);
            _filteredCharacters = new ObservableCollection<Character>(user.CharacterList);
            SearchCommand = new ActionCommand(FilterCharacters);

            //_contextFactory = contextFactory;
        }
        private void FilterCharacters(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchKeyword) || string.IsNullOrWhiteSpace(SelectedCategory))
            {
                FilteredCharacters = new ObservableCollection<Character>(User.CharacterList);
                return;
            }

            var filtered = User.CharacterList.Where(character =>
            {
                return SelectedCategory switch
                {
                    "Name" => character.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase),
                    "Class" => character.Class.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase),
                    "Species" => character.Species.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase),
                    "Background" => character.Background.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase),
                    _ => true
                };
            });

            FilteredCharacters = new ObservableCollection<Character>(filtered);
        }
    }
}
