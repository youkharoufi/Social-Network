using Facebook.Data;
using Facebook.Interface;
using Facebook.Models;
using Facebook.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FriendsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IFriendsRepository _friendsRepository;

        public FriendsController(UserManager<User> userManager, DataContext context, IFriendsRepository friendsRepository)
        {
            _userManager = userManager;
            _context = context;
            _friendsRepository = friendsRepository;
        }


        [HttpPost("sendRequest/{username}/{targetusername}")]
        public async Task<IActionResult> SendFriendRequest(string username, string targetusername)
        {
            var sourceUser = await _friendsRepository.GetUserByUsername(username);
            var targetUser = await _friendsRepository.GetUserByUsername(targetusername);

            var friendingResquest = await _friendsRepository.GetFriendRequestByIds(sourceUser.Id, targetUser.Id);

            if (friendingResquest != null) return BadRequest("You already sent this user an invitation");

            var friendRequest = new FriendRequest
            {
                SourceUserId = sourceUser.Id,
                TargetUserId = targetUser.Id,
                SourceUser = sourceUser,
                TargetUser = targetUser,
            };

            await _friendsRepository.AddFriendRequest(targetUser, friendRequest);

            await _friendsRepository.SaveAllWUserAsync(targetUser);
            await _friendsRepository.SaveAllAsync();


            return Ok(targetUser);

        }

        [HttpPost("confirmation/{username}/{otherUserId}")]

        public async Task<ActionResult<User>> ConfirmFriendRequest(string username, string otherUserId)
        {
            var meUser = await _friendsRepository.GetUserByUsernameWithFrAndPhotos(username);
            var friendRequest = await _friendsRepository.GetFriendRequestByIds(otherUserId, meUser.Id);

            var targetUser = await _friendsRepository.GetUserById(otherUserId);


            if (friendRequest == null) return NotFound();

            if (meUser.Friends.Contains(targetUser)) return BadRequest("You are already Friends with this person");

            meUser.FriendRequestsReceived.Remove(friendRequest);



            meUser.Friends.Add(targetUser);
            targetUser.Friends.Add(meUser);


            await _friendsRepository.SaveAllWUserAsync(targetUser);
            await _friendsRepository.SaveAllWUserAsync(meUser);

            await _friendsRepository.SaveAllAsync();

            return meUser;

        }

        [HttpPost("cancel/{username}/{otherUsername}")]
        public async Task<ActionResult<User>> DeclineInvitation(string username, string otherUsername)
        {
            var meUser = await _friendsRepository.GetUserByUsername(username);
            var otherUser = await _friendsRepository.GetUserByUsername(otherUsername);

            var friendRequest = await _friendsRepository.GetFriendRequestByIds(otherUser.Id, meUser.Id);

            meUser.FriendRequestsReceived.Remove(friendRequest);

            await _friendsRepository.SaveAllAsync();

            await _friendsRepository.SaveAllWUserAsync(meUser);

            return meUser;

        }

        [HttpGet("amis/{username}")]
        public async Task<ActionResult<List<User>>> GetListOfFriends(string username)
        {
            var user = await _friendsRepository.GetUserByUsernameWithFriends(username);

            return user.Friends;
        }

        [HttpGet("pasAmis/{username}")]
        public async Task<List<User>> GetAllNotFriends(string username)
        {
            var user = await _friendsRepository.GetUserByUsernameWithFriends(username);

            var allUsers = await _friendsRepository.GetAllOtherUsers(username);

            var listNotFriends = new List<User>();

            foreach(var notFriend in allUsers)
            {
                if (!user.Friends.Contains(notFriend))
                {
                    listNotFriends.Add(notFriend);
                }
            }

            return listNotFriends;
        }

        [HttpGet("friendRequests/{meId}")]
        public async Task<List<FriendRequest>> GetAllFriendRequests(string meId)
        {
            var friendReq = await _friendsRepository.GetAllFriendRequests();

            var liste = new List<FriendRequest>();

            foreach(var req in friendReq)
            {
                if(req.TargetUserId == meId)
                {
                    liste.Add(req);
                }
            }

            return liste;
        }

        [HttpGet("count")]
        public async Task<int> GetAllFriendRequests()
        {
            var fr = await _friendsRepository.GetAllFriendRequests();

            return fr.Count;
        }
    }
}


