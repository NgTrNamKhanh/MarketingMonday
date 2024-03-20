namespace Comp1640_Final.DTO.Response
{
    //public class DashboardResponse
    //{
    //    public Dictionary<int, Dictionary<string, int>> DashboardByYear { get; set; }
    //}
    public class DashboardResponse
    {
        public string Year { get; set; }
        public List<Values> Values { get; set; }
    }
    public class Values
    {
        public string Faculty { get; set; }
        public int value { get; set; }
    }
}
