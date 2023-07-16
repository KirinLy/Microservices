using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer2 : IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            await Task.Delay(1);
            Console.WriteLine("Consume " + "basketcheckout-queue2");
        }
    }
}
