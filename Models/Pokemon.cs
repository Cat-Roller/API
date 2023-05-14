namespace Pokemon_Review_App_Web_API_.Models
{
    public class Pokemon
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime birthday { get; set; }
        public ICollection<Review> reviews { get; set; }
        public ICollection<pokemon_owner> owners { get; set; }
        public ICollection<Pokemon_category> categories { get; set; }
    }
}
