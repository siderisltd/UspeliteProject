namespace Uspelite.Services.Data
{
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> repo;

        public CommentsService(IRepository<Comment> repo)
        {
            this.repo = repo;
        }
    }
}
