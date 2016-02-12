namespace Uspelite.Web.Models.Home
{
    using System.Collections.Generic;

    public class HomeIndexViewModel
    {
        public IList<PostViewModel> NewestPosts { get; set; }

        public IList<PostViewModel> HighRatedPosts { get; set; }

        public IList<PostViewModel> MostCommentedPosts { get; set; }

        public IList<PostViewModel> HighRatedInCategory { get; set; }

        public PostViewModel TopArticle { get; set; }
    }
}