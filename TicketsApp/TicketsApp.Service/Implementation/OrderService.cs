using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Domain;
using TicketsApp.Domain.Dto;
using TicketsApp.Repository.Interface;
using TicketsApp.Service.Interface;

namespace TicketsApp.Service.Implementation
{
    public class OrderService : IOrderService
    {

        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IUserRepository userRepository, IRepository<Order> orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }


        public List<Order> GetOrders(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            return (List<Order>)loggedInUser.Orders;
        }



        public OrderDto GetOrderInfo(Guid? orderId)
        {
            var order = _orderRepository.Get(orderId);

            var allTickets = order?.TicketsInOrder?.ToList();

            var totalPrice = allTickets.Select(x => (x.Ticket.Price * x.Quantity)).Sum();

            OrderDto dto = new OrderDto
            {
                Tickets = allTickets,
                TotalPrice = totalPrice
            };
            return dto;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAll().ToList();
        }
    }
}
