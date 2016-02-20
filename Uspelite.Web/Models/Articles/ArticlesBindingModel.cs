namespace Uspelite.Web.Models.Articles
{
    public class ArticlesBindingModel
    {
        public string Title { get; set; }

        public string Slug { get; set; }

        public string Content { get; set; }

        public int CategoryId { get; set; }
    }
}