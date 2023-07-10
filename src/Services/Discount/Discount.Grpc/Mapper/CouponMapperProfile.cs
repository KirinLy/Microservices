using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class CouponMapperProfile : Profile
    {
        public CouponMapperProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
