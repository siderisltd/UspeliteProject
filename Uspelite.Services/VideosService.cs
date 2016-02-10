namespace Uspelite.Services.Data
{
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class VideosService : IVideosService
    {
        private readonly IRepository<Video> repo;

        public VideosService(IRepository<Video> repo)
        {
            this.repo = repo;
        }
    }
}
