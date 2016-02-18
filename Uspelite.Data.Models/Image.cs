namespace Uspelite.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Text;
    using BaseModels;
    using BaseModels.Contracts;
    using Common;
    using Enum;

    public class Image : CommentableRateableBaseModel, IBaseModel, ICommentableEntity, IRateableEntity, IAuditInfo, IDeletableEntity
    {

        private string title;

        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(300)]
        [Required]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                var words = value.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var cycleCount = words.Length < Constants.MAX_WORDS_IN_IMAGE_NAME ? words.Length : Constants.MAX_WORDS_IN_IMAGE_NAME;
                StringBuilder sb = new StringBuilder();
                var randomGen = new RandomGenerator();
                var randomStr = randomGen.RandomString(1, 5);
                sb.Append(randomStr);
                sb.Append('_');
                for (int i = 0; i < cycleCount; i++)
                {
                    sb.Append(words[i]);
                    if(i != cycleCount - 1)
                    {
                        sb.Append('-');
                    }
                }
                this.title = sb.ToString();
            }
        }

        public ImageType ImageType { get; set; }

        public string AltTag { get; set; }

        public string Url { get; set; }

        public string PathOriginalSize { get; set; }

        public string PathResizedImage { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int? ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        public bool IsMain { get; set; }

        [NotMapped]
        public Stream Stream { get; set; }

    }
}
