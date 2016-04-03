namespace Uspelite.Data.Models.BaseModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Contracts;

    public abstract class CommentableRateableBaseModel : BaseModel, ICommentableRateableBaseModel, ICommentableEntity, IRateableEntity, IBaseModel, IAuditInfo, IDeletableEntity
    {
        protected ICollection<Comment> comments;
        protected ICollection<Rate> ratings;

        protected CommentableRateableBaseModel()
        {
            this.comments = new HashSet<Comment>();
            this.ratings = new HashSet<Rate>();
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Rate> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }

        [NotMapped]
        public virtual float CalculatedRating
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
