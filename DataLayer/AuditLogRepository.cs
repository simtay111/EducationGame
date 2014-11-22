using System;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using NHibernate;

namespace DataLayer
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private ISession _connection;

        public AuditLogRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _connection = _connectionProvider.CreateConnection();
        }

        public void SaveNew(AuditLog auditLog)
        {
            auditLog.TimeStamp = DateTime.Now;
            _connection.SaveOrUpdate(auditLog);
        }
    }
}