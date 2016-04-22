namespace Uspelite.Web.Models.Categories
{
    using System.Collections.Generic;
    using Articles;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public int? ParentId { get; set; }

        public virtual ICollection<ArticleViewModel> Articles { get; set; }
    }
}