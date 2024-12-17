using CharacterManager.Database;
using CharacterManager.Models;
using CharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace CharacterManager.Commands
{
    public class CreateCharacterCommand : CommandBase
    {
        private readonly CreationVM _creationVM;
        private readonly User _user;
        Services.NavigationService _profileNavigation;

        public CreateCharacterCommand(CreationVM creationVM, User user, Services.NavigationService profileNavigation)
        {
            _creationVM = creationVM;
            _user = user;
            _creationVM.PropertyChanged += OnViewModelPropertyChanged;
            _profileNavigation = profileNavigation;
        }

        public override async void Execute(object parameter)
        {
            Character newCharacter = new Character(_creationVM.CharacterName, _creationVM.ChosenClass, _creationVM.ChosenSpecies, _creationVM.ChosenBackground, 1);

            newCharacter.SetAbilities(_creationVM.Strength, _creationVM.Dexterity, _creationVM.Constitution,
               _creationVM.Intelligence, _creationVM.Wisdom, _creationVM.Charisma);
            newCharacter.SetSkills();
            newCharacter.AddSkillProficiency(_creationVM.ClassSkillProf1);
            newCharacter.AddSkillProficiency(_creationVM.ClassSkillProf2);
            newCharacter.ArmorClass = 10 + newCharacter.GetModifier(newCharacter.GetAbility(Ability.Dexterity));
            newCharacter.InitiativeBonus = newCharacter.GetModifier(newCharacter.GetAbility(Ability.Dexterity));
            newCharacter.MaxHealth = newCharacter.Class.HitDice.Roll() + newCharacter.GetModifier(newCharacter.GetAbility(Ability.Constitution));
            newCharacter.CurrentHealth = newCharacter.MaxHealth;

            try
            {
                _user.AddCharacter(newCharacter);
                DbManager.InsertBackground(newCharacter.Background);
                //DbManager.InsertCharacter(_user, newCharacter); //VERY CAREFUL. STATIC CLASS WASN'T TESTED!!!
                DbManager.InsertCharacter(_user, newCharacter);
                MessageBox.Show("Successfully Created a Character.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                _profileNavigation.Navigate(_user);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't create Character.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CreationVM.CanCreateCharacter))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
