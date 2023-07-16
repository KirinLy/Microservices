namespace EventBus.Messages.Common
{
    public class RabbitMqSettings
    {
        public string Uri { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string BasketCheckoutQueue1 { get; set; } = null!;
        public string BasketCheckoutQueue2 { get; set; } = null!;
    }
}
