using Pokemon_Review_App_Web_API_.Data;
using Pokemon_Review_App_Web_API_.Interfaces;
using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(p => p.id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            var pokemons = _context.Pokemon_categories.Where(pc => pc.CategoryId == category.id);
            foreach (var pokemon_category in pokemons)
            {
                _context.Remove(pokemon_category);    
            }
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
             
            return _context.Categories.OrderBy(p=>p.id).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(p=>p.id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            var pokemons = _context.Pokemon_categories.Where(p=>p.CategoryId==categoryId).Select(p=>p.pokemon).ToList();
            return pokemons;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
