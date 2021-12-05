using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache e_redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            e_redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasket(string userName)
        {
            await e_redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await e_redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await e_redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }
    }
}