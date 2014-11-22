namespace DataLayer
{
    public class HistoryRepository 
    {
        private readonly IConnectionProvider _connectionProvider;

        public HistoryRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
    }
}