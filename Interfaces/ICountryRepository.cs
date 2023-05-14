using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersByCountry(int countryId);
        bool CountryExists(int id);

        bool CreateCountry(Country country);
        bool Save();

        bool UpdateCountry(Country country);

        bool DeleteCountry(Country country);
    }
}
