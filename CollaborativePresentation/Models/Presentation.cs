using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativePresentation.Models
{
    public class Presentation
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; } = string.Empty;  // Initialize with empty string

        [Required]
        public string CreatorName { get; set; } = string.Empty;  // Initialize with empty string

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Slide> Slides { get; set; } = new List<Slide>();
    }

    public class Slide
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string PresentationId { get; set; } = string.Empty;  // Initialize with empty string

        [ForeignKey("PresentationId")]
        public Presentation Presentation { get; set; } = null!;  // Null-forgiving operator

        public int Order { get; set; }

        public ICollection<TextElement> TextElements { get; set; } = new List<TextElement>();
    }

    public class TextElement
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string SlideId { get; set; } = string.Empty;  // Initialize with empty string

        [ForeignKey("SlideId")]
        public Slide Slide { get; set; } = null!;  // Null-forgiving operator

        public string Content { get; set; } = string.Empty;  // Initialize with empty string

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Width { get; set; } = 200;
        public int Height { get; set; } = 100;
    }

    public class UserConnection
    {
        [Key]
        public string ConnectionId { get; set; } = string.Empty;  // Initialize with empty string

        [Required]
        public string PresentationId { get; set; } = string.Empty;  // Initialize with empty string

        [Required]
        public string UserName { get; set; } = string.Empty;  // Initialize with empty string

        public bool IsEditor { get; set; } = false;
    }
}