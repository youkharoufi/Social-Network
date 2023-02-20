namespace Facebook.Models
{
    public class PhotoDto
    {
        public string UserId { get; set; }

        public IFormFile fileMain { get; set; }
        public IFormFile fileSecond { get; set; }
        public IFormFile fileThird { get; set; }

    }
}
