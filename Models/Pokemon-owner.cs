namespace Pokemon_Review_App_Web_API_.Models
{
    public class pokemon_owner
    {
        public int PokemonId { get; set; }
        public int OwnerId { get; set; }
        public Pokemon pokemon { get; set; }
        public Owner owner { get; set; }
    }
}
