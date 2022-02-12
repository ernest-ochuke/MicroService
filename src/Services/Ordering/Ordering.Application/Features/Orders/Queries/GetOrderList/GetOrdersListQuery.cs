using System;
using System.Collections.Generic;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrdersListQuery : IRequest<List<OrdersVm>>
    {
         public string UserName { get; set; }
        public GetOrdersListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentException(nameof(userName));

        }
    
    }
}