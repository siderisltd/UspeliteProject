namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using DTO;
    using Uspelite.Data.Models;

    public interface IArticlesService
    {
        IQueryable<Article> GetNewestPosts(int count = 6, string category = null);

        IQueryable<Article> GetTopPostsByRating(int count = 6, string category = null);

        IQueryable<Article> GetMostCommented(int count = 6, string category = null);

        IQueryable<CategoryAndPostsDTO> GetTopCountPostsByRatingInEveryCategory(int count = 3, IEnumerable<Category> categories = null);

        IQueryable<Article> GetByTitle(string title);
    }
}
