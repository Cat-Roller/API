using Pokemon_Review_App_Web_API_.Models;

namespace Pokemon_Review_App_Web_API_.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool CategoryExists(int categoryId);

        bool CreateCategory(Category category);
        bool Save();

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);
    }
}
