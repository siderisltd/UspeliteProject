namespace Uspelite.Web.Models.Categories
{
    using System.Collections.Generic;
    using Data.Models;
    using Home;
    using Infrastructure.Mapping.Contracts;

    public class CategoriesViewModel : IMapFrom<Category>, IMapTo<Category>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public IList<PostViewModel> Posts { get; set; }
    }
}