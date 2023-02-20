using System.Security.Claims;

namespace Facebook.ClaimsUsers
{
    public static class ClaimsPrincipalExtension
    {

        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
    
}
