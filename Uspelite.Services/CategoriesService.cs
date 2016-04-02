namespace Uspelite.Services.Data
{
    using System;
    using System.Linq;
    using Contracts;
    using DTO;
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

        public IQueryable<Category> GetBySlug(string slug)
        {
            var result = this.repo.All().Where(x => x.Slug == slug);

            return result;
        }

        public IQueryable<PagedCategoryDTO> GetPaged(string slug, int page, int pageCount)
        {
            var itemsToSkip = (page - 1) * pageCount;

            var result = this.GetBySlug(slug)
                .Select(x => new PagedCategoryDTO
                {
                    Title = x.Title,
                    AllItemsCount = x.Articles.Count,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(x.Articles.Count / (decimal) pageCount),
                    Articles = x.Articles.OrderByDescending(z => z.CreatedOn).Skip(itemsToSkip).Take(pageCount).AsQueryable()
                }).AsQueryable();

            return result;
        }

        public Category GetById(int id)
        {
            return this.repo.GetById(id);
        }
    }
}
