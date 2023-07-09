using System.Data;
using Dapper;
using Npgsql;

namespace Discount.API.Data
{
    public class DiscountContext : IDiscountContext
    {
        private readonly string _connectionString;

        public DiscountContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        private async Task<IDbConnection>GetOpenConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using var conn = await GetOpenConnectionAsync();
            return await conn.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using var conn = await GetOpenConnectionAsync();
            return await conn.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using var conn = await GetOpenConnectionAsync();
            return await conn.ExecuteAsync(sql, parameters);
        }
    }
}
