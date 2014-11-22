using System;
using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IReceiptRepository
    {
        void Save(Receipt model);
        void Delete(int id);
        List<Receipt> GetInRange(int acctInfoId, DateTime start, DateTime end);
    }
}