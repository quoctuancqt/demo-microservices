namespace Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EntityBase : IEntity, IAudit
    {
        [Key]
        public virtual string Id { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime? ModifiedDate { get; set; }
    }
}
