namespace Facebook.Models
{
    public class MessageSent
    {
        public string SenderUsername { get; set; }
        public string targetId { get; set; }

        public string Content { get; set; }

        public DateTime messageSent { get; set; }
    }
}
