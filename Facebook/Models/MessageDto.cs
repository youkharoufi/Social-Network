using System.Text.Json.Serialization;

namespace Facebook.Models
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }

        public string SenderUsername { get; set; }
        [JsonIgnore]
        public User Sender { get; set; }

        public string TargetId { get; set; }

        public string TargetUsername { get; set; }
        [JsonIgnore]
        public User Target { get; set; }

        public string Content { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime MessageSent { get; set; }
    }
}
