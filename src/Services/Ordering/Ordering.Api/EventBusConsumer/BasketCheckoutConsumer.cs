using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using System;
using System.Threading.Tasks;

namespace Ordering.Api.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper e_Mapper;
        private readonly IMediator e_Mediator;
        private readonly ILogger<BasketCheckoutConsumer> e_Logger;

        public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
        {
            e_Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            e_Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            e_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = e_Mapper.Map<CheckoutOrderCommand>(context.Message);

            var result = await e_Mediator.Send(command);

            e_Logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);


        }
    }
}
