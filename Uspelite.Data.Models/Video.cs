namespace Uspelite.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BaseModels;
    using BaseModels.Contracts;

    public class Video : CommentableRateableBaseModel, ISeoEntity, IBaseModel, ICommentableEntity, IRateableEntity, IAuditInfo, IDeletableEntity
    {
        private ICollection<Articles_Videos> articlesVideos;

        public Video()
        {
            this.articlesVideos = new HashSet<Articles_Videos>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Title { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        [Required]
        public string Slug { get; set; }

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

        public virtual ICollection<Articles_Videos> ArticlesVideos
        {
            get { return this.articlesVideos; }
            set { this.articlesVideos = value; }
        }
    }
}
