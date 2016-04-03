namespace Uspelite.Web.Models.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class ArticlesBindingModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("[^А-я ]", ErrorMessage = "В заглавието е позволена само кирилица")]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        [AllowHtml]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public HttpPostedFileBase TitleImage { get; set; }

        [Required]
        public bool ImportBranding { get; set; }

        [Required]
        public bool MirrorFlip { get; set; }

        public IEnumerable<SelectListItem> AllCategories { get; set; }

        public string X1 { get; set; }

        public string Y1 { get; set; }

        public string W { get; set; }

        public string H { get; set; }

        public string MainImageInBase64String { get; set; }
    }
}