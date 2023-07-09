using Discount.API.Data;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IDiscountContext _discountContext;

        public CouponRepository(IDiscountContext discountContext)
        {
            _discountContext = discountContext;
        }

        public async Task<Coupon> GetCouponAsync(int id)
        {
            string sql = "Select * From Coupon Where Id = @Id";
            var parameter = new {Id = id};

            return await _discountContext.QueryFirstOrDefaultAsync<Coupon>(sql, parameter);
        }

        public async Task<Coupon> GetCouponByProductNameAsync(string productName)
        {
            string sql = "Select * From Coupon Where ProductName = @ProductName";
            var parameter = new {ProductName = productName};

            return await _discountContext.QueryFirstOrDefaultAsync<Coupon>(sql, parameter);
        }

        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
            string sql = "Insert Into Coupon(Code, ProductName, Discount, Description) " +
                         "Values (@Code, @ProductName, @Discount, @Description) " +
                         "RETURNING Id, Code, ProductName, Discount, Description";
            var parameter = new
            {
                Code = coupon.Code,
                ProductName = coupon.ProductName,
                Discount = coupon.Discount,
                Description = coupon.Description,
            };

            var createdCoupon = await _discountContext.QueryFirstOrDefaultAsync<Coupon>(sql, parameter);
            return createdCoupon;
        }

        public async Task<bool> UpdateCouponAsync(Coupon coupon)
        {
            string sql = "Update Coupon " +
                         "Set Code = @Code, ProductName = @ProductName, Discount = @Discount, Description = @Description " +
                         "Where Id = @Id";
            var parameter = new
            {
                Id = coupon.Id,
                Code = coupon.Code,
                ProductName = coupon.ProductName,
                Discount = coupon.Discount,
                Description = coupon.Description,
            };

            var affected = await _discountContext.ExecuteAsync(sql, parameter);
            return affected > 0;
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {
            string sql = "Delete From Coupon Where Id = @Id";
            var parameter = new { Id = id};

            var affected = await _discountContext.ExecuteAsync(sql, parameter);
            return affected > 0;
        }
    }
}
