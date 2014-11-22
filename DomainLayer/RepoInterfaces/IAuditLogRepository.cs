using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IAuditLogRepository
    {
        void SaveNew(AuditLog auditLog);
    }
}