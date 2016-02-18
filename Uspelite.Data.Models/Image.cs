﻿namespace Uspelite.Data.Models
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

    public class Image : CommentableRateableBaseModel, ISeoEntity, IBaseModel, ICommentableEntity, IRateableEntity, IAuditInfo, IDeletableEntity
    {

        private string title;

        public int Id { get; set; }

        [StringLength(300)]
        [Required]
        public string Title { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        [Required]
        public string Slug { get; set; }

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