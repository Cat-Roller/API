namespace Pokemon_Review_App_Web_API_.Models
{
    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<Owner> owners { get; set; }
    }
}
