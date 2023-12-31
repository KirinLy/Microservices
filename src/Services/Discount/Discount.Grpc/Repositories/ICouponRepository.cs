﻿using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public interface ICouponRepository
    {
        public Task<Coupon> GetCouponAsync(int id);
        public Task<Coupon> GetCouponByProductIdAsync(string productId);
        public Task<Coupon> CreateCouponAsync(Coupon coupon);
        public Task<bool> UpdateCouponAsync(Coupon coupon);
        public Task<bool> DeleteCouponAsync(int id);
    }
}
