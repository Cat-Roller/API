namespace Pokemon_Review_App_Web_API_.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<Pokemon_category> pokemons { get; set; }
    }
}
