namespace Uspelite.Services.Data
{
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class PostsService : IPostsService
    {
        private readonly IRepository<Post> repo;

        public PostsService(IRepository<Post> repo)
        {
            this.repo = repo;
        }
    }
}
