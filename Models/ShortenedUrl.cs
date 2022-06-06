using System.ComponentModel.DataAnnotations;

namespace Short.er.Models
{
    public class ShortenedUrl
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Hash { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public int NumberOfRquests { get; set; }
    }
}
