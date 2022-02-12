using AutoMapper;
using Basket.api.Entities;
using EventBus.Messages.Events;

namespace Basket.api.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
