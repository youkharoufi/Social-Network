using Facebook.Models;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Token
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
