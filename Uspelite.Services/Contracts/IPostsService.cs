namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using Uspelite.Data.Models;

    public interface IPostsService
    {
        IQueryable<Post> GetNewestPosts(int count = 6, string category = null);

        IQueryable<Post> GetTopPostsByRating(int count = 6, string category = null);

        IQueryable<Post> GetMostCommented(int count = 6, string category = null);

        IQueryable<IGrouping<ICollection<Category>, Post>> GetTopCountPostsByRatingInEveryCategory(int count = 3, IEnumerable<Category> categories = null);
    }
}
