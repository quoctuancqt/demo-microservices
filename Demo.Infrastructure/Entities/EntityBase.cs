namespace Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EntityBase : IEntity, IAudit
    {
        [Key]
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
