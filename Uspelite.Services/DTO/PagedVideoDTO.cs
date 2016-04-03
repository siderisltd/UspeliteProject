namespace Uspelite.Services.Data.DTO
{
    using System.Linq;
    using Uspelite.Data.Models;

    public class PagedVideoDTO
    {
        public IQueryable<Video> Videos { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int AllItemsCount { get; set; }
    }
}
