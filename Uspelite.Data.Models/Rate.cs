namespace Uspelite.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BaseModels;

    public class Rate : BaseModel
    {
        public int Id { get; set; }

        public bool IsPositive { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public int? CommentId { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        public int? VideoId { get; set; }

        [ForeignKey("VideoId")]
        public virtual Video Video { get; set; }

        public int? PictureId { get; set; }

        [ForeignKey("PictureId")]
        public virtual Image Image { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int? ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}
