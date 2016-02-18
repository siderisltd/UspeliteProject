namespace Uspelite.Web.Models.Categories
{
    using System.Collections.Generic;
    using Data.Models;
    using Home;
    using Infrastructure.Mapping.Contracts;

    public class CategoryViewModel : IMapFrom<Category>, IMapTo<Category>
    {
        public string Title { get; set; }

        public IEnumerable<ArticleViewModel> Articles { get; set; }

        public ArticleViewModel TopArticle { get; set; }
    }
}