namespace Uspelite.Web.Models.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class ArticlesBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        [AllowHtml]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public HttpPostedFileBase TitleImage { get; set; }

        [Required]
        public bool ImportBranding { get; set; }

        [Required]
        public bool MirrorFlip { get; set; }

        public IEnumerable<SelectListItem> AllCategories { get; set; }
    }
}