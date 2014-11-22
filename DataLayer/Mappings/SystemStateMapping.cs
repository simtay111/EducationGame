using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class SystemStateMapping : ClassMap<SystemState>
    {
        public SystemStateMapping()
        {
            Id(x => x.Id);
            Map(x => x.LastDateChecked);
        } 
    }
}