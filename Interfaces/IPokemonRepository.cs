using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExists(int id);

        bool createPokemon(int ownerId,int categoryId,Pokemon pokemon);
        bool Save();

        bool UpdatePokemon(Pokemon pokemon);

        bool DeletePokemon(Pokemon pokemon);
    }
}
