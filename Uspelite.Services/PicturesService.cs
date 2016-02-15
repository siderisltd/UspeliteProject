namespace Uspelite.Services.Data
{
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class PicturesService : IPicturesService
    {
        private readonly IRepository<Picture> repo;

        public PicturesService(IRepository<Picture> repo)
        {
            this.repo = repo;
        }
    }
}
