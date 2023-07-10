using Discount.Grpc.Protos;

namespace Basket.API.Services;

public interface ICouponService
{
    public Task<CouponModel> GetCouponByProductIdAsync(string  productId);
}