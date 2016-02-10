namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enum;

    public class Picture
    {
        private ICollection<Rate> rates;

        public Picture()
        {
            this.rates = new HashSet<Rate>();
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(300)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        //TODO: Add picture resizer with fixed 800 maxSize
        public ImageType ImageType { get; set; }

        public string AltTag { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsMainPicture { get; set; }

        public virtual ICollection<Rate> Rates
        {
            get { return this.rates; }
            set { this.rates = value; }
        }
    }
}
