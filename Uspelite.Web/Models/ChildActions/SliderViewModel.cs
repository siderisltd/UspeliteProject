namespace Uspelite.Web.Models.ChildActions
{
    using System.Collections.Generic;
    using Articles;

    public class SliderViewModel
    {
        public IList<ArticleViewModel> Posts { get; set; }
    }
}