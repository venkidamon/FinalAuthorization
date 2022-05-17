using Authorization.Entities;

namespace Authorization.Repository
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByNameAsync(string name);
            
    }
}
