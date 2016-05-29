namespace Uspelite.Web.Models.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using System.Text.RegularExpressions;
    using Data.Models.Enum;
    using System.ComponentModel;

    public class ArticlesBindingModel : IMapFrom<Article>, IValidatableObject
    {
        public ArticlesBindingModel()
        {
            this.Status = PostStatus.Draft;
        }

        public int Id { get; set; }

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

        [DefaultValue(PostStatus.Draft)]
        public PostStatus Status { get; set; }

        public string X1 { get; set; }

        public string Y1 { get; set; }

        public string W { get; set; }

        public string H { get; set; }

        public string MainImageInBase64String { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var isInvalid = true;
            if (!string.IsNullOrEmpty(this.Title))
            {
                isInvalid = Regex.IsMatch(this.Title, "[^А-я A-z0-9\"'$%#&№)(+=!?,.;:/-]");
            }

            if (isInvalid)
            {
                yield return new ValidationResult("В заглавието има не позволени символи");
            }

        }
    }
}