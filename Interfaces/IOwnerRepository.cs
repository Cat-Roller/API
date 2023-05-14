using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnersByPokemon(int id);
        ICollection<Pokemon> GetPokemonsByOwner(int id);
        bool OwnerExists(int id);

        bool createOwner(Owner owner);
        bool Save();

        bool UpdateOwner(Owner owner);

        bool DeleteOwner(Owner owner);
    }
}
