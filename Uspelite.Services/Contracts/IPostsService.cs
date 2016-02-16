namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using DTO;
    using Uspelite.Data.Models;

    public interface IPostsService
    {
        IQueryable<Post> GetNewestPosts(int count = 6, string category = null);

        IQueryable<Post> GetTopPostsByRating(int count = 6, string category = null);

        IQueryable<Post> GetMostCommented(int count = 6, string category = null);

        IQueryable<CategoryAndPostsDTO> GetTopCountPostsByRatingInEveryCategory(int count = 3, IEnumerable<Category> categories = null);

        IQueryable<Post> GetByTitle(string title);
    }
}
