namespace Uspelite.Web.Models.Categories
{
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class CategoryInfoViewModel : IMapFrom<Category>
    {
        public string Title { get; set; }

        public string Slug { get; set; }

        public int HomePriority { get; set; }
    }
}