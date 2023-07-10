using Discount.Grpc.Protos;
using Grpc.Core;

namespace Basket.API.Services
{
    public class CouponService : ICouponService
    {
        private readonly CouponProtoService.CouponProtoServiceClient _couponProtoServiceClient;

        public CouponService(CouponProtoService.CouponProtoServiceClient couponProtoServiceClient)
        {
            _couponProtoServiceClient = couponProtoServiceClient;
        }

        public async Task<CouponModel> GetCouponByProductIdAsync(string productId)
        {
            return await _couponProtoServiceClient.GetCouponByProductAsync(new GetCouponByProductRequest()
            {
                ProductId = productId
            });
        }
    }
}
