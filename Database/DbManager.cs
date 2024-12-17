using CharacterManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManager.Database
{
    public static class DbManager
    {
        public static bool DeleteCharacterQuery(int characterId)
        {
            using (var context = new CharacterManagerDbContext())
            {
                try
                {
                // Load the character with all related entities if necessary
                var character = context.Characters
                    .Include(c => c.AbilityScores)
                    .Include(c => c.CharacterItems)
                    .Include(c => c.Spells)
                    .Include(c => c.Traits)
                    .FirstOrDefault(c => c.Id == characterId);

                if (character == null)
                {
                    Console.WriteLine("Character not found");
                    return false;
                }

                // Remove related data first (if needed)
                context.AbilityScores.RemoveRange(character.AbilityScores);
                context.CharacterItems.RemoveRange(character.CharacterItems);

                // Remove the character itself
                context.Characters.Remove(character);

                // Save changes
                context.SaveChanges();

                Console.WriteLine("Character deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the character: {ex.Message}");
                return false;
            }
        }
        }


        public static List<Class> GetAllClassesQuery()
        {
            using (var context = new CharacterManagerDbContext())
            {
                return context.Classes
                    .Include(c => c.StartingGoldDice)
                    .Include(c => c.HitDice)
                    .Include(c => c.user)
                    .Include(c => c.Traits)

                    .Include(c => c.ClassProficiencies)
                        .ThenInclude(cp => cp.Proficiency)
                    .Include(c => c.Proficiencies)

                    .Include(c => c.ClassItems)
                        .ThenInclude(cp => cp.Item)
                    .Include(c => c.Items)

                    .Include(c => c.ClassSpells)
                        .ThenInclude(cp => cp.Spell)
                    .Include(c => c.Spells)
                    .ToList();
            }
        }

        public static List<Species> GetAllSpeciesQuery()
        {
            using (var context = new CharacterManagerDbContext())
            {
                return context.Species
                    .Include(s => s.Traits)
                    .ToList();
            }
        }

        public static User GetUserByLoginQuery(string login, string password)
        {
            using (var context = new CharacterManagerDbContext())
            {
                // Load user and their characters along with related data
                var user = context.Users
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.Class)
                                          .ThenInclude(cls => cls.Traits) // Include Class Traits
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.Class)
                                          .ThenInclude(cls => cls.ClassItems)
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.Class)
                                          .ThenInclude(cls => cls.ClassSpells)
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.Class)
                                          .ThenInclude(c => c.ClassProficiencies)
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.Species)
                                          .ThenInclude(s => s.Traits) // Include Species Traits
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.Background)
                                          .ThenInclude(bg => bg.BackgroundItems)
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.Background)
                                          .ThenInclude(bg => bg.BackgroundTrait) // Include Background Trait
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.CharacterItems)
                                          .ThenInclude(ci => ci.Item)
                                  .Include(u => u.CharacterList)
                                      .ThenInclude(c => c.AbilityScores)
                                  .FirstOrDefault(u => u.UserName == login);

                // Verify user existence and password
                if (user == null || !VerifyPassword(password, user.Password))
                {
                    return new User();
                }

                return user;
            }
        }


        public static Background InsertBackground(Background background)
        {
            using (var context = new CharacterManagerDbContext())
            {
                try
                {
                    // Check for tracking issues and detach if necessary
                    var trackedBackground = context.ChangeTracker.Entries<Background>()
                                                   .FirstOrDefault(e => e.Entity.Id == background.Id);
                    if (trackedBackground != null)
                    {
                        trackedBackground.State = EntityState.Detached;
                    }

                    // Attach related entities if they are not new
                    if (background.BackgroundTrait != null && background.BackgroundTrait.Id > 0)
                    {
                        context.Entry(background.BackgroundTrait).State = EntityState.Unchanged;
                    }

                    foreach (var item in background.Items)
                    {
                        if (item.Id > 0)
                        {
                            context.Entry(item).State = EntityState.Unchanged;
                        }
                    }

                    foreach (var bgItem in background.BackgroundItems)
                    {
                        if (bgItem.Item != null && bgItem.Item.Id > 0)
                        {
                            context.Entry(bgItem.Item).State = EntityState.Unchanged;
                        }
                    }

                    // Attach the background object to the DbContext
                    context.Entry(background).State = background.Id == 0 ? EntityState.Added : EntityState.Modified;

                    // Save changes
                    context.SaveChanges();
                    return background;
                }
                catch (Exception ex)
                {
                    // Log or handle exception
                    Console.WriteLine($"Error inserting background: {ex.Message}");
                    throw;
                }
            }
        }

        public static void InsertCharacter(User user, Character character)
        {
            using (var context = new CharacterManagerDbContext())
            {
                try
                {
                    var trackedCharacter = context.ChangeTracker.Entries<Character>()
                       .FirstOrDefault(e => e.Entity.Id == character.Id);
                    if (trackedCharacter != null)
                    {
                        trackedCharacter.State = EntityState.Detached;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting character: {ex.Message}");
                    throw;
                }

                try
                {
                    // Attach or set related entities
                    if (character.Class != null && character.Class.Id > 0)
                    {
                        context.Entry(character.Class).State = EntityState.Unchanged;
                    }
                    if (character.Species != null && character.Species.Id > 0)
                    {
                        context.Entry(character.Species).State = EntityState.Unchanged;
                    }
                    if (character.Background != null && character.Background.Id > 0)
                    {
                        context.Entry(character.Background).State = EntityState.Unchanged;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting character: {ex.Message}");
                    throw;
                }

                try
                {
                    character.UserId = user.Id;
                    // First, insert the character and get the generated Id
                    context.Entry(character).State = character.Id == 0 ? EntityState.Added : EntityState.Modified;
                    context.SaveChanges();  // This will generate the Id for the character

                    // Now that we have the generated Id, handle character items
                    foreach (var characterItem in character.CharacterItems.ToList())
                    {
                        if (characterItem.Item != null && characterItem.Item.Id > 0)
                        {
                            context.Entry(characterItem.Item).State = EntityState.Unchanged;
                        }

                        // Use the generated character.Id
                        var trackedItem = context.CharacterItems.FirstOrDefault(ci => ci.CharacterId == character.Id && ci.ItemId == characterItem.Item.Id);

                        if (trackedItem != null)
                        {
                            // Remove and re-add if a key update is needed
                            context.CharacterItems.Remove(trackedItem);
                            context.SaveChanges();
                        }

                        // Add new character item
                        context.CharacterItems.Add(characterItem);
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting character items: {ex.Message}");
                    throw;
                }

                try
                {
                    foreach (var abilityScore in character.AbilityScores)
                    {
                        context.Entry(abilityScore).State = abilityScore.Id == 0 ? EntityState.Added : EntityState.Modified;
                    }

                    // Attach or update the Character entity
                    context.Entry(character).State = character.Id == 0 ? EntityState.Added : EntityState.Modified;

                    // Attach or update the User entity
                    var trackedUser = context.ChangeTracker.Entries<User>()
                        .FirstOrDefault(e => e.Entity.Id == user.Id);
                    if (trackedUser != null)
                    {
                        trackedUser.State = EntityState.Detached;
                    }
                    context.Entry(user).State = user.Id == 0 ? EntityState.Added : EntityState.Modified;

                    // Save changes
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting character: {ex.Message}");
                    throw;
                }
            }
        }







        public static List<Item> GetCharacterItems(int characterId)
        {
            using (var context = new CharacterManagerDbContext())
            {
                // Retrieve the character, including related data
                var character = context.Characters
                    .Include(c => c.Class)
                        .ThenInclude(cls => cls.ClassItems)
                            .ThenInclude(ci => ci.Item)
                    .Include(c => c.Background)
                        .ThenInclude(bg => bg.BackgroundItems)
                            .ThenInclude(bi => bi.Item)
                    .Include(c => c.CharacterItems)
                        .ThenInclude(ci => ci.Item)
                    .FirstOrDefault(c => c.Id == characterId);

                if (character == null)
                {
                    throw new ArgumentException($"Character with ID {characterId} not found.");
                }

                // Retrieve the character's current items
                var characterItems = character.CharacterItems.Select(ci => ci.Item).ToList();

                // Check for missing ClassItems
                foreach (var classItem in character.Class.ClassItems)
                {
                    if (!characterItems.Any(ci => ci.Id == classItem.Item.Id))
                    {
                        // Add the missing item to CharacterItems
                        var newCharacterItem = new CharacterItem
                        {
                            CharacterId = character.Id,
                            ItemId = classItem.Item.Id,
                            Item = classItem.Item
                        };

                        context.CharacterItems.Add(newCharacterItem);
                        characterItems.Add(classItem.Item);
                    }
                }

                // Check for missing BackgroundItems
                foreach (var backgroundItem in character.Background.BackgroundItems)
                {
                    if (!characterItems.Any(ci => ci.Id == backgroundItem.Item.Id))
                    {
                        // Add the missing item to CharacterItems
                        var newCharacterItem = new CharacterItem
                        {
                            CharacterId = character.Id,
                            ItemId = backgroundItem.Item.Id,
                            Item = backgroundItem.Item
                        };

                        context.CharacterItems.Add(newCharacterItem);
                        characterItems.Add(backgroundItem.Item);
                    }
                }

                // Save any changes to the database
                context.SaveChanges();

                return characterItems;
            }
        }

        public static List<Proficiency> GetClassProfsQuery(int classId)
        {
            using (var context = new CharacterManagerDbContext())
            {
                return context.ClassProficiencies
                    .Where(cp => cp.ClassesId == classId)
                    .Include(cp => cp.Proficiency) // Ensures Proficiency is loaded
                    .Select(cp => cp.Proficiency)
                    .ToList();
            }
        }


        public static void InsertUserQuery(User user)
        {
            using (var context = new CharacterManagerDbContext())
            {
                if (context.Users.Any(u => u.UserName == user.UserName))
                {
                    throw new Exception("A user with this username already exists.");
                }

                user.Password = HashPassword(user.Password);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static IEnumerable<Item> GetAllItemsQuery()
        {
            using (var context = new CharacterManagerDbContext())
            {
                return context.Items.ToList();
            }
        }

        // Helper method to hash a password
        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        public static IEnumerable<Trait> GetAllTraitsQuery()
        {
            using (var context = new CharacterManagerDbContext())
            {
                return context.Traits.ToList();
            }
        }

        public static IEnumerable<Background> GetAllBackgroundsQuery()
        {
            using (var context = new CharacterManagerDbContext())
            {
                return context.Backgrounds
                    .Include(b => b.BackgroundTrait) // Include the related Trait
                    .Include(b => b.BackgroundItems)
                        .ThenInclude(bi => bi.Item) // Include the items for the background
                    .Include(b => b.Items) // Include the directly related Items
                    .ToList();
            }
        }
    }

}
