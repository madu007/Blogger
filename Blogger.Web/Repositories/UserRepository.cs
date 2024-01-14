using Blogger.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            _authDbContext=authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await _authDbContext.Users.ToListAsync();

            var superAdminUser = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Email == "superAdminBlogger.com");

            if (superAdminUser != null)
            {
                users.Remove(superAdminUser);
            }
            return users;
        }
    }
}
