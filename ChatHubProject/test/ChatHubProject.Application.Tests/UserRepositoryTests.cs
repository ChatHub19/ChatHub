using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Infrastructure.Repositories;
using ChatHubProject.Application.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChatHubProject.Application.Tests
{
    [Collection("Sequential")]
    public class UserRepositoryTests
    {
        public ChatHubContext GetDatabase()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlServer("Server=127.0.0.1,11433;Initial Catalog=ChatHubDb;User Id=sa;Password=SqlServer2019;TrustServerCertificate=true")
                .Options;
            var db = new ChatHubContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        [Fact]
        public async void DeleteUserTest()
        {
            // ARRANGE
            var db = GetDatabase();
            var repo = new UserRepository(db);

            var user = new User(
                username: "TestUser",
                password: "password",
                email: "email@gmail.com",
                role: "Pupil",
                group: null);
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // ACT
            await repo.Delete(user.Guid);

            // ASSERT
            Assert.True(db.Users.Count() == 0);
        }
    }
}
