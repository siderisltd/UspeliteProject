namespace Uspelite.Web.Models.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using Home;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;

    public class PageableCategoryArticlesViewModel : IMapFrom<PagedCategoryDTO>
    {
        public string Title { get; set; }

        public IEnumerable<ArticleViewModel> Articles { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int AllItemsCount { get; set; }

        public string Slug { get; set; }

        public int PageSize { get; set; }
    }
}