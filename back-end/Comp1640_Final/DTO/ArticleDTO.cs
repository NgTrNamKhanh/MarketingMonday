namespace Comp1640_Final.DTO
{
    public class ArticleDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int FacultyId { get; set; }
        public string StudentId { get; set; }
        public string ImagePath { get; set; }
        public string DocPath { get; set; }
        public List<byte[]> ImageBytes { get; set; }

        //public string MarketingCoordinatorId { get; set; }
    }
}
