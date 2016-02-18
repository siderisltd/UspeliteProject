namespace Uspelite.Services.Data.DTO
{
    using System.Collections.Generic;
    using System.Linq;
    using Uspelite.Data.Models;

    public class PagedCategoryDTO
    {
        public string Title { get; set; }

        public IQueryable<Article> Articles { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int AllItemsCount { get; set; }
    }
}
