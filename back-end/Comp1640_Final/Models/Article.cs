using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Comp1640_Final.Models
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? CoordinatorComment { get; set; }
        public DateTime UploadDate { get; set; }
        public int PublishStatusId { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFiles {  get; set; } 
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile DocFiles { get; set; }
        public string? DocPath { get; set; }

        [ForeignKey("FacultyId")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        [ForeignKey("EventId")]
        public int? EventId { get; set; }
        public Event Event { get; set; }

        [ForeignKey("StudentId")]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [ForeignKey("MarketingCoordinatorId")]
        public string? MarketingCoordinatorId { get; set; }
        public ApplicationUser MarketingCoordinator { get; set; }
    }

    public class ListArticleDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int FacultyId { get; set; }
        public string StudentId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public IFormFile DocFiles { get; set; }
    }
}
