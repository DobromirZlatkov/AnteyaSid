namespace AnteyaSidOnContainers.Services.Catalog.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class AuditInfo : IAuditInfo
    {
        public DateTime CreatedOn { get; set; }
        
        public DateTime? ModifiedOn { get; set; }
    }
}
