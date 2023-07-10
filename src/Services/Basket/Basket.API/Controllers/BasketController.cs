using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Basket.API.Services;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository, ICouponService couponService)
        {
            _basketRepository = basketRepository;
            _couponService = couponService;
        }


        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            foreach (var item in basket?.Items ?? Enumerable.Empty<ShoppingCartItem>())
            {
                var coupon = await _couponService.GetCouponByProductIdAsync(item.ProductId);
                if (coupon != null)
                {
                    item.Price -= coupon.Discount;
                }
            }
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadGateway)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            var basket = await _basketRepository.UpdateBasket(shoppingCart);
            return Ok(basket);
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
    }
}
