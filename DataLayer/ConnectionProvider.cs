using DataLayer.Configuration;
using NHibernate;

namespace DataLayer
{
    public class ConnectionProvider : IConnectionProvider
    {
        public ISession CreateConnection()
        {
            return NHibernateHelper.GetCurrentSession();
        }
    }
}