using Facebook.Models;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Interface
{
    public interface IFriendsRepository
    {
        Task<User> GetUserByUsername(string username);


        Task<User> GetUserById(string id);


        Task<User> GetUserByUsernameWithFrAndPhotos(string username);


        Task<User> GetUserByUsernameWithFriends(string username);

        Task<List<User>> GetAllOtherUsers(string username);


        Task<List<FriendRequest>> GetAllFriendRequests();



        Task SaveAllWUserAsync(User user);


        Task SaveAllAsync();

        Task<ActionResult<User>> AddFriendRequest(User user, FriendRequest fr);


        Task<FriendRequest> GetFriendRequestByIds(string sourceUserId, string targetUserId);







    }
}
