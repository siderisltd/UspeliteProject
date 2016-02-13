namespace Uspelite.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;
    using Contracts;
    using Microsoft.AspNet.Identity;
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
