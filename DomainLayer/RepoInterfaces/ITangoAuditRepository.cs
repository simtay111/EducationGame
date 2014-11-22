using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface ITangoAuditRepository
    {
        void SaveNew(TangoAudit audit);
        void Update(TangoAudit auditLog);
    }
}