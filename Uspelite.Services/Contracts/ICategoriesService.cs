namespace Uspelite.Services.Data.Contracts
{
    using System.Linq;
    using Uspelite.Data.Models;
    using DTO;

    public interface ICategoriesService
    {
        IQueryable<Category> GetAll();

        int Add(Category category);

        IQueryable<Category> GetBySlug(string title);

        IQueryable<PagedCategoryDTO> GetPaged(string slug, int page, int pageCount);

        Category GetById(int id);

        IQueryable<Category> GetParentCategories();
    }
}
