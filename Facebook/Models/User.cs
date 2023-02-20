using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Models
{
    public class User : IdentityUser
    {
        public List<Photo> Photos { get; set; }

        public string MainPhotoUrl { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public bool IsConnected { get; set; }

        public List<FriendRequest> FriendRequestsReceived { get; set; } = new List<FriendRequest>();

        public List<User> Friends { get; set; } = new List<User>();

        public List<Message> MessagesSent { get; set; }

        public List<Message> MessagesRecieved { get; set; }
    }
}
