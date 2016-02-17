namespace Uspelite.Data.Models.BaseModels.Contracts
{
    using System.Collections.Generic;

    public interface ICommentableEntity
    {
        ICollection<Comment> Comments { get; set; }
    }
}
