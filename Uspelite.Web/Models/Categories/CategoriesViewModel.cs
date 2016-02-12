namespace Uspelite.Web.Models.Categories
{
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class CategoriesViewModel : IMapFrom<Category>, IMapTo<Category>
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}