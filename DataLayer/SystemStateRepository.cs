using System;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DataLayer
{
    public class SystemStateRepository : ISystemStateRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public SystemStateRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public SystemState Get()
        {
            var connection = _connectionProvider.CreateConnection();
            var state = connection.Get<SystemState>(1);
            if (state == null)
            {
                state = new SystemState { LastDateChecked = DateTime.Now.AddDays(-2) };
                Save(state);
            }
            return state;
        }

        public void Save(SystemState state)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(state);
            ;
        }
    }
}