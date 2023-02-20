using Facebook.Data;
using Facebook.Interface;
using Facebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repositories
{
    public class FriendsRepository : IFriendsRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public FriendsRepository(UserManager<User> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return user;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task<User> GetUserByUsernameWithFrAndPhotos(string username)
        {
            var user = await _userManager.Users.Include(p=>p.Friends).Include(o=>o.Photos).FirstOrDefaultAsync(u=>u.UserName == username);

            return user;
        }

        public async Task<User> GetUserByUsernameWithFriends(string username)
        {
            var user = await _userManager.Users.Include(p => p.Friends).FirstOrDefaultAsync(u => u.UserName == username);

            return user;
        }

        public async Task<List<User>> GetAllOtherUsers(string username)
        {
            var users = await _userManager.Users.Where(o => o.UserName != username).ToListAsync();

            return users;
        }

        public async Task<List<FriendRequest>> GetAllFriendRequests()
        {
            var frs = await _context.FriendRequests.Include(p => p.SourceUser).Include(p => p.TargetUser).ToListAsync();

            return frs;

        }


        public async Task SaveAllWUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
            
        }

        public async Task SaveAllAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<User>> AddFriendRequest(User user, FriendRequest fr)
        {
            user.FriendRequestsReceived.Add(fr);

            return user;

            
        }


        public async Task<FriendRequest> GetFriendRequestByIds(string sourceUserId, string targetUserId)
        {
            var friendingResquest = await _context.FriendRequests.Where(i => (i.SourceUserId == sourceUserId) && (i.TargetUserId == targetUserId)).SingleOrDefaultAsync();

            return friendingResquest;

        }

        
    }
}
