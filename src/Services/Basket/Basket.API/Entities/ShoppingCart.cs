namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public int TotalPrice { get; set; }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
    }
}
