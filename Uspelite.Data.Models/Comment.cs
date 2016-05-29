namespace Uspelite.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BaseModels;
    using BaseModels.Contracts;
    using Common;

    public class Comment : CommentableRateableBaseModel, IBaseModel, ICommentableEntity, IRateableEntity, IAuditInfo, IDeletableEntity
    {
        private ICollection<Comment> children;

        public Comment()
        {
            this.children = new HashSet<Comment>();
        }

        public int Id { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Comment Parent { get; set; }

        public int? ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        public int? VideoId { get; set; }

        [ForeignKey("VideoId")]
        public virtual Video Video { get; set; }

        public int? ImageId { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

        [InverseProperty(nameof(Parent))]
        public virtual ICollection<Comment> Children
        {
            get { return this.children; }
            set { this.children = value; }
        }
    }
}
