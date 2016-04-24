namespace Uspelite.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Models.Enum;
    using Uspelite.Data.Repositories;
    using Uspelite.Web.Infrastructure.Enums;
    using System.Data.Entity;
    using Fissoft.EntityFramework.Fts;
    using NinjaNye.SearchExtensions;

    public class ArticlesService : IArticlesService
    {
        private readonly IRepository<Article> repo;
        private readonly ICategoriesService categoriesService;
        private readonly IImagesService imagesService;

        public ArticlesService(IRepository<Article> repo, ICategoriesService categoriesService, IImagesService imagesService)
        {
            this.repo = repo;
            this.categoriesService = categoriesService;
            this.imagesService = imagesService;
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

        public int Add(string title, string slug, string authorId, string content, PostStatus status, int categoryId, Image image, IList<Comment> comments = null)
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

            if (comments != null)
            {
                article.Comments = comments;
            }

            this.repo.Add(article);
            this.repo.SaveChanges();

            return article.Id;
        }

        public int Update(int id, string title, string authorId, string content, PostStatus status, int categoryId, Image image)
        {
            var article = this.repo.GetById(id);
            if (article != null)
            {
                article.Title = title;
                article.AuthorId = authorId;
                article.Content = content;
                article.Status = status;
                article.CategoryId = categoryId;
                if (image != null)
                {
                    this.imagesService.RemoveAllRelatedToArticle(id);
                    article.Images = new List<Image> { image };
                }
            }
            this.repo.SaveChanges();

            return article?.Id ?? 0;
        }

        public bool Exists(string title)
        {
            return this.repo.All().Any(x => x.Title == title);
        }

        public IQueryable<ArticlePlaceTypeDTO> GetPossibleArticlePlaces()
        {
            var result = new List<ArticlePlaceTypeDTO>();

            foreach (ArticlePlaceType place in Enum.GetValues(typeof(ArticlePlaceType)))
            {
                if (place != ArticlePlaceType.Normal)
                {
                    result.Add(new ArticlePlaceTypeDTO
                    {
                        Id = (int)place,
                        Name = place.ToString()
                    });
                }
            }

            return result.AsQueryable();
        }

        public IQueryable<ArticleStatusDTO> GetPossibleArticleStatuses()
        {
            var result = new List<ArticleStatusDTO>();

            foreach (PostStatus status in Enum.GetValues(typeof(PostStatus)))
            {
                result.Add(new ArticleStatusDTO
                {
                    Id = (int)status,
                    Name = status.ToString()
                });
            }

            return result.AsQueryable();
        }

        public IQueryable<Article> All()
        {
            return this.repo.All();
        }

        public Comment AddCommentTo(Article foundArticle, Comment commentToAdd)
        {
            foundArticle.Comments.Add(commentToAdd);
            this.repo.SaveChanges();
            return commentToAdd;
        }

        public Article GetById(int id)
        {
            return this.repo.All().FirstOrDefault(x => x.Id == id);
        }

        public void DeleteById(int id)
        {
            this.repo.Delete(id);
        }

        public int SaveChanges()
        {
            return this.repo.SaveChanges();
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

        public IQueryable<Article> GetMostCommented(int count = 6, string categoryTitle = null)
        {
            var query = this.repo.All();

            if (!string.IsNullOrEmpty(categoryTitle))
            {
                query = query.Where(x => x.Category.Title == categoryTitle);
            }
            query = query
                   .OrderByDescending(x => x.Comments.Count)
                   .Take(count);

            return query;
        }

        public IQueryable<CategoryAndPostsDTO> GetTopArticles(ArticleTopFactor topFactor = ArticleTopFactor.Rating, int count = 3, IEnumerable<Category> categories = null)
        {
            if (categories == null)
            {
                //TODO: Fix this bullshit
                categories = this.categoriesService.GetAll().AsEnumerable();
            }

            var query = this.repo
                               .All()
                               .Where(x => categories.Contains(x.Category));

            if (topFactor == ArticleTopFactor.Rating)
            {
                query = query.OrderByDescending(x => x.Ratings.Sum(y => y.Value)/x.Ratings.Count);
            }
            else if(topFactor == ArticleTopFactor.Newest)
            {
                query = query.OrderByDescending(x => x.CreatedOn);
            }

            var newQuery = query
                            .GroupBy(x => x.Category)
                            .Select(x => new CategoryAndPostsDTO { Category = x.Key, Posts = x.Take(count) });

            return newQuery;
        }

        public IQueryable<Article> GetBySlug(string slug)
        {
            var query = this.repo
                            .All()
                            .Where(x => x.Slug == slug);

            return query;
        }

        public IQueryable<Article> GetAllFilteredByTitle(string searchQuery)
        {
            var query = this.repo.All();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query
                    .Where(x => x.Title.ToLower().Contains(searchQuery.ToLower()) ||
                            x.Content.ToLower().Contains(searchQuery.ToLower()));
            }

            return query;
        }

        public IQueryable<Article> FullTextSearch(string query)
        {
            var result = this.repo
                             .All()
                             .Search(x => x.Title.ToLower())
                             .Containing(query.ToLower())
                             .ToRanked()
                             .OrderByDescending(r => r.Hits)
                             .Select(x => x.Item)
                             .Union(this.repo.All()
                                        .Search(x => x.Content.ToLower())
                                        .Containing(query.ToLower())
                                        .ToRanked()
                                        .OrderByDescending(x => x.Hits)
                                        .Select(x => x.Item));

            return result;
        }
    }
}
