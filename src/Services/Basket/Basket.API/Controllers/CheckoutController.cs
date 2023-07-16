using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc; 

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IBus _bus;


        public CheckoutController(IBasketRepository basketRepository, IMapper mapper, IPublishEndpoint publishEndpoint, IBus bus)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _bus = bus;
        }

        [HttpPost()]
        [ProducesResponseType((int) StatusCodes.Status200OK)]
        [ProducesResponseType((int) StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // Get basket
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName );
            if (basket == null)
                return BadRequest();

            // Create event
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basketCheckout.TotalPrice;
            var en = await _bus.GetSendEndpoint(new Uri("queue:basketcheckout-queue1"));
            await en.Send(eventMessage);
            //await  _publishEndpoint.Publish(eventMessage, ctx =>
            //{
            //    ctx.DestinationAddress = new Uri("queue:basketcheckout-queue2");
            //});

            // Remove basket
            //await _basketRepository.DeleteBasket(basketCheckout.UserName);

            return Ok();
        }
    }
}
