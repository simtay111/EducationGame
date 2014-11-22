using System;
using DomainLayer;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DataLayer
{
    public class TangoAuditRepository : ITangoAuditRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public TangoAuditRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void SaveNew(TangoAudit auditLog)
        {
            auditLog.TimeStamp = DateTime.Now;
            auditLog.Status = PaymentStatus.Created;
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(auditLog);
        }
        public void Update(TangoAudit auditLog)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Update(auditLog);
        }
    }
}