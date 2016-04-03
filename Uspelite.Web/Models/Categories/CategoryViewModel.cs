namespace Uspelite.Web.Models.Categories
{
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}