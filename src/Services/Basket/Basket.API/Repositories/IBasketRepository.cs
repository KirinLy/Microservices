using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        public Task<ShoppingCart> GetBasket(string userName);
        public Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart);
        public Task DeleteBasket(string userName);
    }
}
