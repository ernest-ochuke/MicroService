using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.api.Entities;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository e_repository;
        private readonly DiscountGrpcService e_discountGrpcService;
        private readonly IMapper e_mapper;
        private readonly IPublishEndpoint e_publishEndPoint;
      

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            e_repository = repository ?? throw new ArgumentException(nameof(repository));
            e_discountGrpcService = discountGrpcService ?? throw new ArgumentException(nameof(discountGrpcService));

            e_mapper = mapper ?? throw new ArgumentException(nameof(mapper));

            e_publishEndPoint = publishEndpoint ?? throw new ArgumentException(nameof(publishEndpoint));
            
        }
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await e_repository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await e_discountGrpcService.GetDiscount(item.ProductName);
                item.Price -=coupon.Amount;
                //item.ProductName;
            }
            return Ok(await e_repository.UpdateBasket(basket));
        }
        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await e_repository.DeleteBasket(userName);
            return Ok();
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price 
            // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket

            // get existing basket with total price
            var basket = await e_repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = e_mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await e_publishEndPoint.Publish(eventMessage);

            // remove the basket
            await e_repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}