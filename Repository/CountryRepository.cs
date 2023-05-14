using Pokemon_Review_App_Web_API_.Data;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext dataContext)
        {
            _context=dataContext;
        }

        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c=>c.id==id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {

            _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c=>c.id).ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.id==id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int id)
        {
            return _context.Owners.Where(o => o.id == id).Select(o => o.country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersByCountry(int id)
        {
            return _context.Owners.Where(o=>o.country.id==id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}
