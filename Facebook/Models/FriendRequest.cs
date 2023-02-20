using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }

        public string SourceUserId { get; set; }

        public User SourceUser { get; set; }

        public string TargetUserId { get; set; }

        public User TargetUser { get; set; }
    }
}
