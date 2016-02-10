namespace Uspelite.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rate
    {
        public int Id { get; set; }

        public bool IsPositive { get; set; }

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
        public virtual Picture Picture { get; set; }

        public int? PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }
}
