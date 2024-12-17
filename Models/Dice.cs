using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManager.Models
{
    public enum DiceType
    {
        [Display(Name = "d4")]
        D4 = 4,
        [Display(Name = "d6")]
        D6 = 6,
        [Display(Name = "d8")]
        D8 = 8,
        [Display(Name = "d10")]
        D10 = 10,
        [Display(Name = "d12")]
        D12 = 12,
        [Display(Name = "d20")]
        D20 = 20,
        [Display(Name = "d100")]
        D100 = 100
    }

    [Table("Dice")]
    public class Dice
    {
        [Key]
        public int Id { get; set; } // Primary key

        public DiceType Type { get; set; }
        public int NumberOfDice { get; set; }
        public bool IsHitDie { get; set; }
        public bool IsGoldDie { get; set; }

        // Nullable foreign keys and navigation properties
        public int? BelongToClassId { get; set; }
        public Class? BelongToClass { get; set; }

        //public int? BelongToCharacterId { get; set; }
        //public Character? BelongToCharacter { get; set; }

        public Dice(DiceType type, int numberOfDice)
        {
            Type = type;
            NumberOfDice = numberOfDice;
            IsHitDie = false;
            IsGoldDie = false;
        }

        public int Roll()
        {
            Random random = new Random();
            int total = 0;

            for (int i = 0; i < NumberOfDice; i++)
            {
                total += random.Next(1, (int)Type + 1);
            }

            return total;
        }

        public static int Roll(DiceType type, int numberOfDice)
        {
            Random random = new Random();
            int total = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                total += random.Next(1, (int)type + 1);
            }

            return total;
        }

        public static int RollAbility()
        {
            Random random = new Random();
            List<int> results = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                results.Add(random.Next(1, (int)DiceType.D6) + 1);
            }

            // Sort the dice in ascending order
            results.Sort();

            // Sum the highest 3 values (discard the lowest one)
            int result = results[1] + results[2] + results[3];

            return result;
        }
    }


}
