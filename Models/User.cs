using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CharacterManager.Models
{
    [Table("Users")]
    public class User
    {
        // Primary key
        [Key]
        public int Id { get; set; }

        // User details
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = "DefaultName";  // Default user name

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        // Navigation properties for relationships (EF Core will create the appropriate foreign key relationships)
        public List<Character> CharacterList { get; set; } = new List<Character>();
        public List<Class> CreatedClasses { get; set; } = new List<Class>();
        public List<Species> CreatedSpecies { get; set; } = new List<Species>();
        public List<Item> CreatedItems { get; set; } = new List<Item>();
        public List<Trait> CreatedFeats { get; set; } = new List<Trait>();

        // Default constructor
        public User()
        {
            Id = 0;
            UserName = "DefaultName";
            CharacterList = new List<Character>();
            CreatedClasses = new List<Class>();
            CreatedSpecies = new List<Species>();
            CreatedItems = new List<Item>();
            CreatedFeats = new List<Trait>();
        }

        // Constructor with UserName
        public User(string userName)
        {
            UserName = userName;
            CharacterList = new List<Character>();
            CreatedClasses = new List<Class>();
            CreatedSpecies = new List<Species>();
            CreatedItems = new List<Item>();
            CreatedFeats = new List<Trait>();
        }
        public User(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
            CharacterList = new List<Character>();
            CreatedClasses = new List<Class>();
            CreatedSpecies = new List<Species>();
            CreatedItems = new List<Item>();
            CreatedFeats = new List<Trait>();
        }

        // Constructor with UserName and List<Character>
        public User(string userName, List<Character> userCharacters)
        {
            UserName = userName;
            CharacterList = userCharacters;
            CreatedClasses = new List<Class>();
            CreatedSpecies = new List<Species>();
            CreatedItems = new List<Item>();
            CreatedFeats = new List<Trait>();
        }

        public User(string userName, List<Character> characterList,
                    List<Class> createdClasses, List<Species> createdSpecies, List<Item> createdItems,
                    List<Trait> createdFeats)
        {
            UserName = userName;
            CharacterList = characterList;
            CreatedClasses = createdClasses;
            CreatedSpecies = createdSpecies;
            CreatedItems = createdItems;
            CreatedFeats = createdFeats;
        }

        // Method to add a character
        public void AddCharacter(Character character)
        {
            this.CharacterList.Add(character);
        }
    }
}
