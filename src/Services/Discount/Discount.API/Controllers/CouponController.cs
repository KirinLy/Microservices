using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<CouponController> _logger;

        public CouponController(ICouponRepository couponRepository, ILogger<CouponController> logger)
        {
            _couponRepository = couponRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Coupon), (int)StatusCodes.Status200OK)]
        [ProducesResponseType((int)StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> GetCoupon([FromRoute] int id)
        {
            var coupon = await _couponRepository.GetCouponAsync(id);
            if (coupon == null)
            {
                _logger.LogInformation($"Coupon with Id: {id} not exist");
                return NotFound();
            }

            return Ok(coupon);
        }

        [HttpGet("productName")]
        [ProducesResponseType(typeof(Coupon), (int)StatusCodes.Status200OK)]
        [ProducesResponseType((int)StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> GetCouponByProductName([FromQuery]string productName)
        {
            var coupon = await _couponRepository.GetCouponByProductNameAsync(productName);
            if (coupon == null)
            {
                _logger.LogInformation($"Coupon with ProductName: {productName} not exist");
                return NotFound();
            }

            return Ok(coupon);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Coupon), (int) StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> CreateCoupon([FromBody]Coupon coupon)
        {
            var createdCoupon = await _couponRepository.CreateCouponAsync(coupon);
            return CreatedAtAction(nameof(GetCoupon), new {id = createdCoupon.Id}, createdCoupon);
        }

        [HttpPut()]
        [ProducesResponseType(typeof(Coupon), (int) StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateCoupon([FromBody]Coupon coupon)
        {
            return Ok(await _couponRepository.UpdateCouponAsync(coupon));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Coupon), (int) StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteCoupon([FromRoute] int id)
        {
            return  Ok(await _couponRepository.DeleteCouponAsync(id));
        }
    }
}
