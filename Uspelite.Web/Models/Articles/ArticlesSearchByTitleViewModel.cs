namespace Uspelite.Web.Models.Articles
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ArticlesSearchByTitleViewModel
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string ToolTip { get; set; }

        public ICollection<SelectListItem> Articles { get; set; }

        public bool OpenInNewWindow { get; set; }
    }
}