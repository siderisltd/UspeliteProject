﻿namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enum;

    public class Post
    {
        private ICollection<Comment> comments;

        private ICollection<Category> categories;

        private ICollection<Rate> rates;

        public Post()
        {
            this.rates = new HashSet<Rate>();
            this.categories = new HashSet<Category>();
            this.comments = new HashSet<Comment>();
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }

        [Index(IsUnique = true)]
        [StringLength(300)]
        [Required]
        public string Route { get; set; }

        [Required]
        [MaxLength(20000, ErrorMessage = "Maximum article content is 20000", ErrorMessageResourceType = typeof(ArgumentException))]
        public string Content { get; set; }

        public int MainPictureId { get; set; }

        [ForeignKey("MainPictureId")]
        public virtual Picture MainPicture { get; set; }

        [DefaultValue(PostStatus.Draft)]
        public PostStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Category> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }

        public virtual ICollection<Rate> Rates
        {
            get { return this.rates; }
            set { this.rates = value; }
        }

        [NotMapped]
        public float CalculatedRating
        {
            get
            {
                float sum = 0.0f;
                int count = 0;
                foreach (Rate rating in this.Rates)
                {
                    sum += rating.Value;
                    count++;
                }

                return sum / count;
            }
        }
    }
}

