using System.Data;

namespace Discount.Grpc.Data
{
    public interface IDiscountContext
    {
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null);
        public Task<int> ExecuteAsync(string sql, object parameters = null);
    }
}
