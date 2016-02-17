namespace Uspelite.Data.Models.BaseModels.Contracts
{
    using System.Collections.Generic;

    public interface IRateableEntity
    {
        ICollection<Rate> Ratings { get; set; }

        float CalculatedRating { get; }
    }
}
