namespace Pokemon_Review_App_Web_API_.Models
{
    public class Pokemon_category
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon pokemon { get; set; }
        public Category category { get; set; }
    }
}
