namespace Uspelite.Services.Data
{
    using System.Linq;
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;


    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> repo;

        public CategoriesService(IRepository<Category> repo)
        {
            this.repo = repo;
        }

        public IQueryable<Category> GetAll()
        {
            return this.repo.All();
        }

        public int Add(Category category)
        {
            this.repo.Add(category);
            this.repo.SaveChanges();
            return category.Id;
        }
    }
}
