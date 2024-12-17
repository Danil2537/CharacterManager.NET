using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CharacterManager.Database
{
    using CharacterManager.Models;
    using Microsoft.EntityFrameworkCore;

    public class CharacterManagerDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Background> Backgrounds { get; set; }
        public DbSet<Character>? Characters { get; set; }
        public DbSet<AbilityScore>? AbilityScores { get; set; } 
        public DbSet<BackgroundItem>? BackgroundItems { get; set; }
        public DbSet<CharacterItem>? CharacterItems { get; set; }

        public DbSet<Class>? Classes { get; set; }
        public DbSet<ClassItem>? ClassItems { get; set; }
        public DbSet<ClassProficiency>? ClassProficiencies { get; set; }
        public DbSet<ClassSpell>? ClassSpells { get; set; }
        public DbSet<Species>? Species { get; set; }
        public DbSet<Item>? Items { get; set; }
        public DbSet<Spell>? Spells { get; set; }
        public DbSet<Trait>? Traits { get; set; }
        public DbSet<Dice>? Dice { get; set; }

        public CharacterManagerDbContext(DbContextOptions<CharacterManagerDbContext> options) : base(options)
        {
        }
        public CharacterManagerDbContext() : base()
        {
        }
        // Configure the connection string and other options
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                // Set the PostgreSQL connection string here
                optionsBuilder
               .UseNpgsql("Server=localhost;Port=5432;Database=CharacterManager;User Id=postgres;Password=30072005;IncludeErrorDetail=True")
               .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Debug).EnableSensitiveDataLogging();
                //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=CharacterManager;User Id=postgres;Password=30072005");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AbilityScore>()
           .HasOne(a => a.Character)
           .WithMany(c => c.AbilityScores)
           .HasForeignKey(a => a.CharacterId);

            // modelBuilder.Entity<BackgroundItem>()
            //.HasKey(bi => new { bi.BackgroundId, bi.ItemId });
            modelBuilder.Entity<Background>()
            .HasMany(e => e.Items)
            .WithMany(e => e.Backgrounds);

            modelBuilder.Entity<BackgroundItem>()
                       .HasKey(cs => new { cs.BackgroundId, cs.ItemId });

            modelBuilder.Entity<BackgroundItem>()
                .HasOne(cs => cs.Background)
                .WithMany(c => c.BackgroundItems)
                .HasForeignKey(cs => cs.BackgroundId);

            modelBuilder.Entity<BackgroundItem>()
                .HasOne(cs => cs.Item)
                .WithMany(s => s.BackgroundItems)
                .HasForeignKey(cs => cs.ItemId);

            modelBuilder.Entity<Character>().Ignore(c => c.ProficiencyBonus);

            modelBuilder.Entity<CharacterItem>()
                        .HasKey(ci => new { ci.CharacterId, ci.ItemId });

            modelBuilder.Entity<CharacterItem>()
                .HasOne(ci => ci.Character)
                .WithMany(c => c.CharacterItems)
                .HasForeignKey(ci => ci.CharacterId);

            modelBuilder.Entity<CharacterItem>()
                .HasOne(ci => ci.Item)
                .WithMany(i => i.CharacterItems)
                .HasForeignKey(ci => ci.ItemId);

            modelBuilder.Entity<Class>()
                        .HasOne(c => c.HitDice)
                        .WithMany()
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                       .HasOne(c => c.StartingGoldDice)
                       .WithMany()
                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>().Ignore(e => e.StartingGold);

            modelBuilder.Entity<ClassProficiency>()
                      .HasKey(cs => new { cs.ClassesId, cs.ProficienciesId });

            modelBuilder.Entity<ClassProficiency>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassProficiencies)
                .HasForeignKey(cs => cs.ClassesId);

            modelBuilder.Entity<ClassProficiency>()
                .HasOne(cs => cs.Proficiency)
                .WithMany(s => s.ClassProficiencies)
                .HasForeignKey(cs => cs.ProficienciesId);

            modelBuilder.Entity<ClassItem>()
          .HasKey(cs => new { cs.ClassesId, cs.ItemsId });

            modelBuilder.Entity<ClassItem>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassItems)
                .HasForeignKey(cs => cs.ClassesId);

            modelBuilder.Entity<ClassItem>()
                .HasOne(cs => cs.Item)
                .WithMany(s => s.ClassItems)
                .HasForeignKey(cs => cs.ItemsId);

            modelBuilder.Entity<ClassSpell>()
            .HasKey(cs => new { cs.ClassesId, cs.SpellsId });

            modelBuilder.Entity<ClassSpell>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassSpells)
                .HasForeignKey(cs => cs.ClassesId);

            modelBuilder.Entity<ClassSpell>()
                .HasOne(cs => cs.Spell)
                .WithMany(s => s.ClassSpells)
                .HasForeignKey(cs => cs.SpellsId);



            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "Admin", Password = "admin123", Email = "admin@example.com" }
            );
        }
    }
}
