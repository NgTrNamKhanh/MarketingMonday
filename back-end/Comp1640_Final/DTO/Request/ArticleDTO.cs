namespace Comp1640_Final.DTO.Request
{
    public class AddArticleDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int FacultyId { get; set; }
        public string StudentId { get; set; }
        public string EventId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public IFormFile? DocFiles { get; set; }

        //public string MarketingCoordinatorId { get; set; }
    }
    public class EditArticleDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int FacultyId { get; set; }
        public string StudentId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public IFormFile DocFiles { get; set; }
    }

    //public class SubmissionDTO
    //{
    //    public Guid Id { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public DateTime UploadDate { get; set; }
    //    public int FacultyId { get; set; }
    //    public string StudentName { get; set; }
    //    public int PublishStatusId { get; set; }

    //    public string CoordinatorComment { get; set; }

    //    public List<byte[]> ImageBytes { get; set; }

    //    //public string MarketingCoordinatorId { get; set; }
    //}


}
