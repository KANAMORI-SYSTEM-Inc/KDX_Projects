using Kdx.Core.Domain.Interfaces;

namespace Kdx.Infrastructure.Adapters
{
    public class AccessRepositoryAdapter : IAccessRepository
    {
        private readonly string _connectionString;

        public AccessRepositoryAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}