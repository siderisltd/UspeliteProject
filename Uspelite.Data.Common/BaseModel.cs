namespace Uspelite.Data.Common
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Contracts;

    public class BaseModel : IBaseModel, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
