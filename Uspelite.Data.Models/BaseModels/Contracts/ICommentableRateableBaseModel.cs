namespace Uspelite.Data.Models.BaseModels.Contracts
{
    using System.Collections.Generic;

    public interface ICommentableRateableBaseModel : IBaseModel, IRateableEntity
    {
        ICollection<Comment> Comments { get; set; }

        ICollection<Rate> Ratings { get; set; }

        float CalculatedRating { get; }
    }
}
