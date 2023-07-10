using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class CouponService : CouponProtoService.CouponProtoServiceBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<CouponService> _logger;
        private readonly IMapper _mapper;

        public CouponService(ICouponRepository couponRepository, ILogger<CouponService> logger, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
        {
            var coupon = await _couponRepository.GetCouponAsync(request.Id);
            if (coupon == null)
            {
                _logger.LogInformation($"Coupon with Id: {request.Id} not exist");
                return new CouponModel();
            }

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var createdCoupon = await _couponRepository.CreateCouponAsync(coupon);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<UpdateCouponRespone> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            return new UpdateCouponRespone()
            {
                Success = await _couponRepository.UpdateCouponAsync(coupon)
            };
        }

        public override async Task<DeleteCouponRespone> DeleteCoupon(DeleteCouponRequest request, ServerCallContext context)
        {
            return new DeleteCouponRespone()
            {
                Success = await _couponRepository.DeleteCouponAsync(request.Id)
            };
        }
    }
}
