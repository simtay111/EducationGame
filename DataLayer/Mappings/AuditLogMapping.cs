using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class AuditLogMapping : ClassMap<AuditLog>
    {
        public AuditLogMapping()
        {
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.TimeStamp);
            Map(x => x.Message);
        } 
    }
}