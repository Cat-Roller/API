namespace Pokemon_Review_App_Web_API_.Models
{
    public class Owner
    {
        public int id { get; set; }
        public string name { get; set; }
        public string gym { get; set; }
        public Country country { get; set; }
        public ICollection<pokemon_owner> pokemons { get; set; }
    }
}
