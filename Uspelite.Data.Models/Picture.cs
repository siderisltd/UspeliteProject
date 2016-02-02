namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
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

        public int Title { get; set; }
        //TODO: Add picture resizer with fixed 800 maxSize
        public ImageType ImageType { get; set; }

        public string AltTag { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Rate> Rates
        {
            get { return this.rates; }
            set { this.rates = value; }
        }
    }
}
