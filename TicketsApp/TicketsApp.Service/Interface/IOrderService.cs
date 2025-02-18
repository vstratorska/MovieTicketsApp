using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Domain;
using TicketsApp.Domain.Dto;

namespace TicketsApp.Service.Interface
{
    public interface IOrderService
    {
        List<Order> GetOrders(string userId);
        OrderDto GetOrderInfo(Guid? orderId);
        List<Order> GetAllOrders();
    }
}
