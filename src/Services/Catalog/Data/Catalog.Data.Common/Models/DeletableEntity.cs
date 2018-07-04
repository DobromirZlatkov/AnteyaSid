namespace AnteyaSidOnContainers.Services.Catalog.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class DeletableEntity : AuditInfo, IDeletableEntity
    {
        public DeletableEntity()
             : base()
        {
        }

        [Editable(false)]
        public bool IsDeleted { get; set; }
        
        [Editable(false)]
        public DateTime? DeletedOn { get; set; }
    }
}
