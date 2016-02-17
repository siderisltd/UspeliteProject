namespace Uspelite.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BaseModels;
    using BaseModels.Contracts;

    public class Video : CommentableRateableBaseModel, IBaseModel, ICommentableEntity, IRateableEntity, IAuditInfo, IDeletableEntity
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
