namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Models.Enum;
    using Uspelite.Web.Infrastructure.Enums;

    public interface IArticlesService
    {
        IQueryable<Article> GetNewestPosts(int count = 6, string category = null);

        IQueryable<Article> GetTopPostsByRating(int count = 6, string category = null);

        IQueryable<Article> GetMostCommented(int count = 6, string category = null);

        IQueryable<CategoryAndPostsDTO> GetTopCountPostsByRatingInEveryCategory(int count = 3, IEnumerable<Category> categories = null);

        IQueryable<CategoryAndPostsDTO> GetTopArticles(ArticleTopFactor topFactor = ArticleTopFactor.Rating, int count = 3, IEnumerable<Category> categories = null);

        IQueryable<Article> GetBySlug(string title);

        IQueryable<Article> GetAllFilteredByTitle(string searchQuery);

        IQueryable<ArticlePlaceTypeDTO> GetPossibleArticlePlaces();

        IQueryable<ArticleStatusDTO> GetPossibleArticleStatuses();

        int Add(string title, string authorId, string content, PostStatus status, int categoryId, Image image);

        int Add(string title, string slug, string authorId, string content, PostStatus status, int categoryId, Image image, IList<Comment> comments = null);

        bool Exists(string title);

        IQueryable<Article> All();

        Comment AddCommentTo(Article foundArticle, Comment commentToAdd);

        Article GetById(int id);

        void DeleteById(int id);

        int SaveChanges();

        int Update(int id, string title, string authorId, string content, PostStatus status, int categoryId, Image image);
    }
}
