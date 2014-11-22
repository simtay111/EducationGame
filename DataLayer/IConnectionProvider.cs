using System.Data;
using NHibernate;

namespace DataLayer
{
    public interface IConnectionProvider
    {
        ISession CreateConnection();
    }
}