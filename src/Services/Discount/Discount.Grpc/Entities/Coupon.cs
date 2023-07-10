namespace Discount.Grpc.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
    }
}
