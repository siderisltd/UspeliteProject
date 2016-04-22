namespace Uspelite.Web.Models.ChildActions
{
    using System.Collections.Generic;
    using Articles;
    using Categories;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;

    public class CategoryAndPostsViewModel : IMapFrom<CategoryAndPostsDTO>
    {
        public CategoryViewModel Category { get; set; }

        public IEnumerable<ArticleViewModel> Posts { get; set; }
    }
}