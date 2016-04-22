namespace Uspelite.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BaseModels;
    using BaseModels.Contracts;

    public class Category : BaseModel, ISeoEntity, IRateableEntity
    {
        private ICollection<Article> articles;

        private ICollection<Video> videos;

        private ICollection<Image> images;

        private ICollection<Rate> ratings;

        private ICollection<Category> children;

        public Category()
        {
            this.videos = new HashSet<Video>();
            this.articles = new HashSet<Article>();
            this.images = new HashSet<Image>();
            this.ratings = new HashSet<Rate>();
            this.children = new HashSet<Category>();
        }

        [ForeignKey(nameof(Parent))]
        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        [Required]
        public string Slug { get; set; }

        [InverseProperty(nameof(Parent))]
        public ICollection<Category> Children
        {
            get { return this.children; }
            set { this.children = value; }
        }

        public ICollection<Article> Articles
        {
            get { return this.articles; }
            set { this.articles = value; }
        }

        public ICollection<Video> Videos
        {
            get { return this.videos; }
            set { this.videos = value; }
        }

        public ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public ICollection<Rate> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }

        [NotMapped]
        public float CalculatedRating
        {
            get
            {
                float sum = 0.0f;
                int count = 0;
                foreach (Rate rating in this.Ratings)
                {
                    sum += rating.Value;
                    count++;
                }

                if(this.Ratings.Count == 0)
                {
                    return 0;
                }

                return sum / count;
            }
        }
    }
}
