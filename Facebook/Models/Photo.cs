using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Facebook.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public bool IsMain { get; set; }

        public bool IsSecond { get; set; }

        public bool IsThird { get; set; }

        [NotMapped]
        public IFormFile fileMain { get; set; }

        [NotMapped]
        public IFormFile fileSecond { get; set; }


        [NotMapped]
        public IFormFile fileThird { get; set; }


        public string UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
