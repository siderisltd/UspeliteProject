namespace Uspelite.Data.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BaseModels;
    using BaseModels.Contracts;
    using Enum;

    public class Article : CommentableRateableBaseModel, ISeoEntity, IBaseModel, ICommentableEntity, IRateableEntity, IAuditInfo, IDeletableEntity
    {
        private ICollection<Image> images;

        public Article()
        {
            this.images = new HashSet<Image>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }


        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [StringLength(200)]
        [Required]
        public string Title { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        [Required]
        public string Slug { get; set; }


        [Required]
        [MaxLength(30000)]
        public string Content { get; set; }

        [Required]
        [DefaultValue(PostStatus.Draft)]
        public PostStatus Status { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        
        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        [NotMapped]
        public Image MainImage
        {
            get { return this.Images.FirstOrDefault(x => x.IsMain); }
        }
    }
}

