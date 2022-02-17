using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient e_Client;

        public OrderService(HttpClient httpClient)
        {
            e_Client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await e_Client.GetAsync($"/Order/{userName}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
