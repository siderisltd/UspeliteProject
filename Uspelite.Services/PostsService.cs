namespace Uspelite.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class PostsService : IPostsService
    {
        private readonly IRepository<Post> repo;
        private readonly ICategoriesService categoriesService;

        public PostsService(IRepository<Post> repo, ICategoriesService categoriesService)
        {
            this.repo = repo;
            this.categoriesService = categoriesService;
        }

        public IQueryable<Post> GetTopPostsByRating(int count = 6, string category = null)
        {
            var query = this.repo.All();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Categories.Any(cat => cat.Title == category));
            }
            query = query
                   .OrderByDescending(x => x.Rates.Sum(y => y.Value))
                   .Take(count);

            return query;
        }

        public IQueryable<Post> GetNewestPosts(int count = 6, string category = null)
        {
            var query = this.repo.All();
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Categories.Any(cat => cat.Title == category));
            }
            query = query
                      .OrderByDescending(x => x.CreatedOn)
                      .Take(count);

            return query;
        }

        public IQueryable<Post> GetMostCommented(int count = 6, string category = null)
        {
            var query = this.repo.All();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Categories.Any(cat => cat.Title == category));
            }
            query = query
                   .OrderByDescending(x => x.Comments.Count)
                   .Take(count);

            return query;
        }

        public IQueryable<CategoryAndPostsDTO> GetTopCountPostsByRatingInEveryCategory(int count = 3, IEnumerable<Category> categories = null)
        {
            if (categories == null)
            {
                categories = this.categoriesService.GetAllCategories().AsEnumerable();
            }

            IQueryable<CategoryAndPostsDTO> query = this.repo
                            .All()
                            .Where(x => x.Categories.Any(y => categories.Contains(y)))
                            .OrderByDescending(x => x.Rates.Sum(y => y.Value))
                            .GroupBy(x => x.Categories.FirstOrDefault().Title)
                            .Select(x => new CategoryAndPostsDTO {CategoryName = x.Key, Posts = x.Take(count)});

            return query;
        }
    }
}
