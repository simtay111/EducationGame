using System;

namespace DomainLayer.Entities
{
    public class AuditLog
    {
        public virtual int Id { get; set; }
        public virtual DateTime TimeStamp { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Message { get; set; } 
    }
}