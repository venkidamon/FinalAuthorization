using Authorization.Entities;

namespace Authorization.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}