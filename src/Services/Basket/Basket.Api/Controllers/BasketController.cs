using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository e_repository;

        public BasketController(IBasketRepository repository)
        {
            e_repository = repository ?? throw new ArgumentException(nameof(repository));
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
            return Ok(await e_repository.UpdateBasket(basket));
        }
        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await e_repository.DeleteBasket(userName);
            return Ok();
        }
        // [Route("[action]")]
        // [HttpPost]
        // [ProducesResponseType((int)HttpStatusCode.Accepted)]
        // [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        // {
        //     // get existing basket with total price 
        //     // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
        //     // send checkout event to rabbitmq
        //     // remove the basket

        //     // get existing basket with total price
        //     var basket = await e_repository.GetBasket(basketCheckout.UserName);
        //     if (basket == null)
        //     {
        //         return BadRequest();
        //     }

        //     // send checkout event to rabbitmq
        //     var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        //     eventMessage.TotalPrice = basket.TotalPrice;
        //     await _publishEndpoint.Publish(eventMessage);

        //     // remove the basket
        //     await _repository.DeleteBasket(basket.UserName);

        //     return Accepted();
        // }
    }
}