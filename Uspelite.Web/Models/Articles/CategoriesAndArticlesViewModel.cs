namespace Uspelite.Web.Models.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using Categories;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;

    public class CategoriesAndArticlesViewModel : IMapFrom<CategoryAndPostsDTO>
    {
        public CategoryInfoViewModel Category { get; set; }

        public IList<ArticleViewModel> Posts { get; set; }

        public int AllSearchedResultsCount { get; set; }
    }
}