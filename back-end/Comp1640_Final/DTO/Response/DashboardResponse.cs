namespace Comp1640_Final.DTO.Response
{
    public class DashboardResponse
    {
        public string Year { get; set; }
        public List<Values> Values { get; set; }
    }
    public class Values
    {
        public string Faculty { get; set; }
        public double value { get; set; }
    }
}
