namespace Uspelite.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Models.Enum;
    using Uspelite.Data.Repositories;

    public class ArticlesService : IArticlesService
    {
        private readonly IRepository<Article> repo;
        private readonly ICategoriesService categoriesService;

        public ArticlesService(IRepository<Article> repo, ICategoriesService categoriesService)
        {
            this.repo = repo;
            this.categoriesService = categoriesService;
        }

        public int Add(string title, string authorId, string content, PostStatus status, int categoryId, Image image)
        {
            var article = new Article
            {
                Title = title,
                AuthorId = authorId,
                Content = content,
                Status = status,
                CategoryId = categoryId,
                Images = new List<Image>() { image }
            };

            this.repo.Add(article);
            this.repo.SaveChanges();

            return article.Id;
        }

        public int Add(string title, string slug, string authorId, string content, PostStatus status, int categoryId, Image image)
        {
            var article = new Article
            {
                Title = title,
                Slug = slug,
                AuthorId = authorId,
                Content = content,
                Status = status,
                CategoryId = categoryId,
                Images = new List<Image>() { image }
            };

            this.repo.Add(article);
            this.repo.SaveChanges();

            return article.Id;
        }

        public bool Exists(string title)
        {
            return this.repo.All().Any(x => x.Title == title);
        }

        public IQueryable<Article> All()
        {
            return this.repo.All();
        }

        public IQueryable<Article> GetTopPostsByRating(int count = 6, string category = null)
        {
            var query = this.repo.All();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.Title == category);
            }
            query = query
                   .OrderByDescending(x => x.Ratings.Sum(y => y.Value) / x.Ratings.Count)
                   .Take(count);

            return query;
        }

        public IQueryable<Article> GetNewestPosts(int count = 6, string category = null)
        {
            var query = this.repo.All();
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.Title == category);
            }
            query = query
                      .OrderByDescending(x => x.CreatedOn)
                      .Take(count);

            return query;
        }

        public IQueryable<Article> GetMostCommented(int count = 6, string category = null)
        {
            var query = this.repo.All();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.Title == category);
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
                categories = this.categoriesService.GetAll().AsEnumerable();
            }

            IQueryable<CategoryAndPostsDTO> query = this.repo
                            .All()
                            .Where(x => categories.Contains(x.Category))
                            .OrderByDescending(x => x.Ratings.Sum(y => y.Value) / x.Ratings.Count)
                            .GroupBy(x => x.Category.Title)
                            .Select(x => new CategoryAndPostsDTO { CategoryName = x.Key, Posts = x.Take(count) });

            return query;
        }

        public IQueryable<Article> GetBySlug(string slug)
        {
            var query = this.repo
                            .All()
                            .Where(x => x.Slug == slug);

            return query;
        }
    }
}
