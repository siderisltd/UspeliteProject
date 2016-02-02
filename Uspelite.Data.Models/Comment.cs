namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment
    {
        private ICollection<Rate> rates;

        public Comment()
        {
            this.rates = new HashSet<Rate>();
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Comment Parent { get; set; }

        public virtual Post Post { get; set; }

        public virtual Video Video { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Rate> Rates
        {
            get { return this.rates; }
            set { this.rates = value; }
        }
    }
}
