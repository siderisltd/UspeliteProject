namespace Uspelite.Web.Models.ChildActions
{
    using System.Collections.Generic;
    using Articles;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;

    public class CategoryAndPostsViewModel : IMapFrom<CategoryAndPostsDTO>
    {
        public string CategoryName { get; set; }

        public IEnumerable<ArticleViewModel> Posts { get; set; }
    }
}