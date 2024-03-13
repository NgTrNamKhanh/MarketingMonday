using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Comp1640_Final.Models
{
    public class Article
    {
        [Key]
        public Guid ArticleId { get; set; }

        [ForeignKey("StudentId")]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [ForeignKey("MarketingCoordinatorId")]
        public string? MarketingCoordinatorId { get; set; }
        public ApplicationUser MarketingCoordinator { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? CoordinatorComment { get; set; }
        public DateTime UploadDate { get; set; }
        public int PublishStatusId { get; set; }

        [ForeignKey("FacultyId")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        [ForeignKey("EventId")]
        public int? EventId { get; set; }
        public Event Event { get; set; }

    }
}
