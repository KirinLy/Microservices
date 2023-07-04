using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories
{
    public class BasketRepository: IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _distributedCache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            await _distributedCache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart));
            return await GetBasket(shoppingCart.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _distributedCache.RemoveAsync(userName);
        }
    }
}
