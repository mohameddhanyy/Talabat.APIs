using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extentions
{
    public static class ManageUserExtension
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U => U.Address).SingleOrDefaultAsync(U => U.Email == email);
            return user;    
        }
    }
}
