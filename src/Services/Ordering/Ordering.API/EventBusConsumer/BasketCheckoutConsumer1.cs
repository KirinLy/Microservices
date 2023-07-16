using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer1 : IConsumer<BasketCheckoutEvent1>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent1> context)
        {
            await Task.Delay(1);
            Console.WriteLine("Consume " + "basketcheckout-queue1");
        }
    }
}
