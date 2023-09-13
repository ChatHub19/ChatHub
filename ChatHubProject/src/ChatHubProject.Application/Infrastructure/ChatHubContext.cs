﻿using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ChatHubProject.Application.Model;

namespace ChatHubProject.Application.Infrastructure
{
    public class ChatHubContext : DbContext
    {
        public ChatHubContext(DbContextOptions opt) : base(opt)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Handin>().HasIndex("TaskId", "StudentId").IsUnique();
            //modelBuilder.Entity<Task>().HasIndex(nameof(Task.Title), "TeamId").IsUnique();
            //// Es sollen DateTimeKind UTC beim zurücklesen gesetzt werden.
            //modelBuilder.Entity<Task>()
            //    .Property(t => t.ExpirationDate)
            //    .HasConversion(
            //        v => v,   // 1:1 in die DB schreiben
            //        v => new DateTime(v.Ticks, DateTimeKind.Utc));  // auslesen als UTC

            // Generic config for all entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }
        }
        private async Task Initialize()
        {
            var users = new[]
            {
                new User(
                    username: "admin",
                    password: "Password1234?",
                    email: "admin@gmail.com",
                    role: Userrole.Admin),
                new User(
                    username: "user",
                    password: "Password1234?",
                    email: "user@gmail.com",
                    role: Userrole.User),
            };
            await Users.AddRangeAsync(users);
            await SaveChangesAsync();
        }

        private async Task Seed()
        {
            Randomizer.Seed = new Random(1039);
            var faker = new Faker("en");

            var users = new Faker<User>("en").CustomInstantiator(f =>
            {
                var username = f.Name.FirstName();
                return new User(
                    username: username.ToLower(),
                    password: "111",
                    email: $"{username.ToLower()}@gmail.com",
                    role: f.PickRandom<Userrole>())
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .GroupBy(a => a.Email).Select(g => g.First())
            .ToList();
            await Users.AddRangeAsync(users);
            await SaveChangesAsync();
        }

        public async Task CreateDatabase(bool isDevelopment)
        {
            if (isDevelopment) { Database.EnsureDeleted(); }
            // EnsureCreated only creates the model if the database does not exist or it has no
            // tables. Returns true if the schema was created.  Returns false if there are
            // existing tables in the database. This avoids initializing multiple times.
            if (Database.EnsureCreated()) { await Initialize(); }
            if (isDevelopment) await Seed();
        }
    }
}