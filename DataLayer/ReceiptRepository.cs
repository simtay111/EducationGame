using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public ReceiptRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void Save(Receipt model)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.SaveOrUpdate(model);
                ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.Delete(connection.Load(typeof(Receipt), id));
                ;
        }

        public List<Receipt> GetInRange(int acctInfoId, DateTime start, DateTime end)
        {
            var connection = _connectionProvider.CreateConnection();
                return (from receipt in connection.Query<Receipt>()
                        where receipt.AccountInformation.Id == acctInfoId
                       && receipt.DateBilled >= start && receipt.DateBilled <= end.Date.AddHours(24)
                        select receipt).ToList();
        }

        public List<Receipt> GetNonSentReceipt()
        {
            var connection = _connectionProvider.CreateConnection();
                return (from receipt in connection.Query<Receipt>()
                        where !receipt.Sent 
                        select receipt).ToList();
        }
    }
}