using System.Linq;
using DomainLayer.Entities;
using NHibernate.Linq;

namespace DataLayer
{
    public class ForgotPasswordTokenRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public ForgotPasswordTokenRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public ForgotPasswordToken GetForUser(string username)
        {
            var connection = _connectionProvider.CreateConnection();
                return
                    (from token in connection.Query<ForgotPasswordToken>()
                     where token.Email == username.ToUpper()
                     select token).FirstOrDefault();
        }

        public void Save(ForgotPasswordToken token)
        {
            var connection = _connectionProvider.CreateConnection();
                var existing =
                    (from tokens in connection.Query<ForgotPasswordToken>()
                     where tokens.Email == token.Email.ToUpper()
                     select tokens).ToList();
                foreach (var tok in existing)
                {
                    connection.Delete(tok);
                }
                connection.SaveOrUpdate(token);
                ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                var token = connection.Get<ForgotPasswordToken>(id);
                connection.Delete(token);
                ;
        }
    }
}