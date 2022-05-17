using Authorization.Data;
using Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == name);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
