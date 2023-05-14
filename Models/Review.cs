namespace Pokemon_Review_App_Web_API_.Models
{
    public class Review
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public decimal rating { get; set; }
        public Pokemon pokemon { get; set; }
        public Reviewer reviewer { get; set; }
    }
}
