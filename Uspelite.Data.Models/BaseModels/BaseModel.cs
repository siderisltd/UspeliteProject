namespace Uspelite.Data.Models.BaseModels
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Contracts;

    public abstract class BaseModel : IBaseModel, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
