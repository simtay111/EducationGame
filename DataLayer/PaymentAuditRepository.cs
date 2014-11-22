using System;
using DomainLayer;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DataLayer
{
    public class PaymentAuditRepository : IPaymentAuditRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public PaymentAuditRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void SaveNew(PaymentAudit auditLog)
        {
            auditLog.Created = DateTime.Now;
            auditLog.Status = PaymentStatus.Created;
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(auditLog);
        }
        public void Update(PaymentAudit auditLog)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Update(auditLog);
        }
    }
}