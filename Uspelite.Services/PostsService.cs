namespace Uspelite.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
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

        public IQueryable<IGrouping<ICollection<Category>, Post>> GetTopCountPostsByRatingInEveryCategory(int count = 3, IEnumerable<Category> categories = null)
        {
            if(categories == null)
            {
                categories = this.categoriesService.GetAllCategories().AsEnumerable();
            }

            IQueryable<IGrouping<ICollection<Category>, Post>> query = this.repo
                .All()
                .OrderByDescending(x => x.Rates.Sum(y => y.Value))
                .Where(x => x.Categories.Any(y => categories.Contains(y)))
                .GroupBy(x => x.Categories)
                .Take(count);

            return query;
        }
    }
}
