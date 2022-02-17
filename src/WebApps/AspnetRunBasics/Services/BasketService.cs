using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient e_Client;

        public BasketService(HttpClient httpClient)
        {
            e_Client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await e_Client.GetAsync($"/Basket/{userName}");
            return await response.ReadContentAs<BasketModel>();
        }
        public async Task<BasketModel> UpdateBasket(BasketModel model)
        {
            var response = await e_Client.PostAsJson($"/Basket", model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<BasketModel>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            var response = await e_Client.PostAsJson($"/Basket/Checkout", model);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }
    }
}
