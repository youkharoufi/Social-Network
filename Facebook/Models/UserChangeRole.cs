namespace Facebook.Models
{
    public class UserChangeRole
    {
        public string UserId { get; set; }
        public string oldRole { get; set; }

        public string newRole { get; set; }
    }
}
