using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IPaymentAuditRepository
    {
        void SaveNew(PaymentAudit auditLog);
        void Update(PaymentAudit auditLog);
    }
}