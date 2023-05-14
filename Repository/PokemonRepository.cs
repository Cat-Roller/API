using Pokemon_Review_App_Web_API_.Data;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        
        public PokemonRepository(DataContext context) 
        { 
            _context = context;
        }

        public bool createPokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var Owner=_context.Owners.Where(o=>o.id==ownerId).FirstOrDefault();
            var Category = _context.Categories.Where(c=>c.id==categoryId).FirstOrDefault();
            Pokemon_category pokemon_Category = new Pokemon_category() 
            { 
                PokemonId=pokemon.id,
                CategoryId=categoryId,
                pokemon = pokemon,
                category=Category,
            };

            _context.Add(pokemon_Category);
            pokemon_owner pokemon_Owner = new pokemon_owner()
            {
                PokemonId = pokemon.id,
                OwnerId=ownerId,
                pokemon = pokemon,
                owner = Owner
            };

            _context.Add(pokemon_Owner);
            _context.Add(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            var categories = _context.Pokemon_categories.Where(pc=>pc.PokemonId==pokemon.id).ToList();
            var owners = _context.Pokemon_owners.Where(po=>po.PokemonId==pokemon.id).ToList();

            foreach (var categoryForPokemon in categories)
            {
                _context.Remove(categoryForPokemon);
            }
            foreach (var OwnerForPokemon in owners)
            {
                _context.Remove(OwnerForPokemon);
            }

            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => p.id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p=>p.name==name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var reviews= _context.Reviews.Where(p=>p.pokemon.id==pokeId);

            if(reviews.Count() <= 0) { return 0; }

            return (decimal)(reviews.Sum(review => review.rating)/reviews.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p=>p.id).ToList();
        }

        public bool PokemonExists(int id)
        {
           return  _context.Pokemons.Any(p => p.id == id);
        }

        public bool Save()
        {
            var saved= _context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
