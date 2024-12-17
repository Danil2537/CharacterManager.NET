using CharacterManager.Commands;
using CharacterManager.Database;
using CharacterManager.Models;
using CharacterManager.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManager.ViewModels
{
    public class RegistrationVM : ViewModelBase
    {
        //private CharacterManagerDbContextFactory _contextFactory;
        private string _errors;
        public string Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                OnPropertyChanged(nameof(Errors));
            }
        }

        private string _enteredUsername = "";
        public string EnteredUsername
        {
            get
            {
                return _enteredUsername;
            }
            set
            {
                _enteredUsername = value;
                OnPropertyChanged(nameof(EnteredUsername));
            }
        }


        private string _enteredEmail = "";
        public string EnteredEmail
        {
            get
            {
                return _enteredEmail;
            }
            set
            {
                _enteredEmail = value;
                OnPropertyChanged(nameof(EnteredEmail));
            }
        }

        private string _enteredPassword = "";
        public string EnteredPassword
        {
            get
            {
                return _enteredPassword;
            }
            set
            {
                _enteredPassword = value;
                OnPropertyChanged(nameof(EnteredPassword));
            }
        }


        private string _repeatedPassword = "";
        public string RepeatedPassword
        {
            get
            {
                return _repeatedPassword;
            }
            set
            {
                _repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
            }
        }
        public CommandBase RegisterCommand { get; }
        public CommandBase LoginCommand { get; }
   
        private void Register(object obj)
        {
            if (EnteredPassword == RepeatedPassword)
            {
                DbManager.InsertUserQuery(new User(EnteredUsername, EnteredPassword, EnteredEmail));
            }
            else { Errors = "Passwords don't match!"; }
        }
        public RegistrationVM(Services.NavigationService loginNavigationService)
        {
            RegisterCommand = new ActionCommand(Register);
            LoginCommand = new NavigateCommand(loginNavigationService);
        }
    }
}
