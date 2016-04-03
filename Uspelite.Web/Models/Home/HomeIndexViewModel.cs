namespace Uspelite.Web.Models.Home
{
    using System.Collections.Generic;
    using Articles;

    public class HomeIndexViewModel
    {
        public IList<ArticleViewModel> NewestPosts { get; set; }

        public IList<ArticleViewModel> HighRatedPosts { get; set; }

        public IList<ArticleViewModel> MostCommentedPosts { get; set; }

        public IList<ArticleViewModel> HighRatedInCategory { get; set; }

        public ArticleViewModel TopArticle { get; set; }
    }
}