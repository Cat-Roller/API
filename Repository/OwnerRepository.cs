using Pokemon_Review_App_Web_API_.Data;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context=context;
        }

        public bool createOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Owner GetOwner(int id)
        {
            return _context.Owners.Where(o => o.id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.OrderBy(o => o.id).ToList();
        }

        public ICollection<Owner> GetOwnersByPokemon(int id)
        {
            return _context.Pokemon_owners.Where(r=>r.PokemonId == id).Select(r=>r.owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int id)
        {
            return _context.Pokemon_owners.Where(r => r.OwnerId == id).Select(r => r.pokemon).ToList();
        }

        public bool OwnerExists(int id)
        {
            return _context.Owners.Any(o=>o.id == id);
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            var pokemons=_context.Pokemon_owners.Where(po=>po.OwnerId==owner.id);
            foreach (var pokemonByOwner in pokemons)
            {
                _context.Remove(pokemonByOwner);
            }

            _context.Remove(owner);
            return Save();
        }
    }
}
