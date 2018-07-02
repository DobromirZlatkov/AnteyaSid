namespace AnteyaSidOnContainers.Services.Catalog.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class AuditInfo : IAuditInfo
    {
        [Timestamp]
        public DateTime CreatedOn { get; set; }

        [Timestamp]
        public DateTime? ModifiedOn { get; set; }
    }
}
