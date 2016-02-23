namespace Uspelite.Services.Data.Contracts
{
    using System.Linq;
    using DTO;
    using Uspelite.Data.Models;

    public interface IVideosService
    {
        IQueryable<Video> All();

        PagedVideoDTO AllPaged(int page, int pageSize, int? categoryId = null);
    }
}
