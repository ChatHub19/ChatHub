using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ChatHubProject.Application.Model;

namespace ChatHubProject.Application.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, int>
    {
        public UserRepository(ChatHubContext db) : base(db)
        {
        }

        public override async Task<(bool success, string message)> Delete(Guid guid)
        {
            var entity = await _db.Users./*Include(h => h.Task).*/FirstOrDefaultAsync(h => h.Guid == guid); //include load related entities along with the main entity --> replace with message, server, ...
            if (entity is null) { return (false, "Handin not found"); }
            return await base.Delete(guid);
        }
        public override async Task<(bool success, string message)> Delete(int id)
        {
            var entity = _db.Users./*Include(h => h.Task).*/FirstOrDefault(h => h.Id == id); //include load related entities along with the main entity --> replace with message, server, ...
            if (entity is null) { return (false, "Handin not found"); }
            return await base.Delete(id);
        }
    }
}
