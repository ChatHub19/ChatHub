using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ChatHubProject.Application.Model;
using Microsoft.VisualBasic;

namespace ChatHubProject.Application.Infrastructure
{
    public class ChatHubContext : DbContext
    {
        public ChatHubContext(DbContextOptions opt) : base(opt)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();

        public DbSet<Server> Servers => Set<Server>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    displayname: "admin",
                    password: "1234",
                    email: "admin@gmail.com",
                    role: Userrole.Administration.ToString()),
                new User(
                    username: "user",
                    displayname: "user",
                    password: "1234",
                    email: "user@gmail.com",
                    role: Userrole.Pupil.ToString()),
                new User(
                    username: "user2",
                    displayname: "user2",
                    password: "1234",
                    email: "user2@gmail.com",
                    role: Userrole.Pupil.ToString()),
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
                    displayname: username.ToLower(),
                    password: "111",
                    email: $"{username.ToLower()}@gmail.com",
                    role: f.PickRandom<Userrole>().ToString())
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .GroupBy(a => a.Email).Select(g => g.First())
            .ToList();
            await Users.AddRangeAsync(users);
            await SaveChangesAsync();

            var message = new Faker<Message>("en").CustomInstantiator(f =>
            {
                return new Message(
                    text: f.Lorem.Word(),
                    user: f.Random.ListItem(users),
                    time: DateTime.Now)
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .ToList();
            await Messages.AddRangeAsync(message);
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
