using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain;
using TicketsApp.Domain.Domain;
using TicketsApp.Domain.Dto;
using TicketsApp.Repository.Interface;
using TicketsApp.Service.Interface;

namespace TicketsApp.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;

        public ShoppingCartService(IRepository<TicketInOrder> ticketInOrderRepository, IRepository<Order> orderRepository, IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository)
        {
            this._ticketInOrderRepository = ticketInOrderRepository;
            this._orderRepository = orderRepository;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
        }
        public bool AddToShoppingConfirmed(TicketInShoppingCart model, string userId)
        {

            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser.ShoppingCart;

            if (userShoppingCart.TicketsInShoppingCarts == null)
                userShoppingCart.TicketsInShoppingCarts = new List<TicketInShoppingCart>(); 

            userShoppingCart.TicketsInShoppingCarts.Add(model);
            _shoppingCartRepository.Update(userShoppingCart);
            return true;
        }

        public bool DeleteTicketFromShoppingCart(string userId, Guid ticketId)
        {
            if (ticketId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                var ticket = userShoppingCart.TicketsInShoppingCarts.Where(x => x.TicketId == ticketId).FirstOrDefault();

                userShoppingCart.TicketsInShoppingCarts.Remove(ticket);

                _shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;

        }

        public ShoppingCartDto GetShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser?.ShoppingCart;
            var allTickets = userShoppingCart?.TicketsInShoppingCarts?.ToList();

            var totalPrice = allTickets.Select(x => (x.Ticket.Price * x.Quantity)).Sum();

            ShoppingCartDto dto = new ShoppingCartDto
            {
                Tickets = allTickets,
                TotalPrice = totalPrice
            };
            return dto;
        }

        public bool Order(string userId)
        {
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    OwnerId = userId,
                    Owner = loggedInUser,
                    TicketsInOrder = new List<TicketInOrder>()
                };

                _orderRepository.Insert(order);

                List<TicketInOrder> ticketsInOrder = new List<TicketInOrder>();

                ticketsInOrder = userShoppingCart.TicketsInShoppingCarts.Select(
                    x => new TicketInOrder
                    {
                        Id = Guid.NewGuid(),
                        TicketId = x.Ticket.Id,
                        Ticket = x.Ticket,
                        OrderId = order.Id,
                        Order = order,
                        Quantity = x.Quantity
                    }
                    ).ToList();

                foreach (var ticket in ticketsInOrder)
                {
                    _ticketInOrderRepository.Insert(ticket);
                }

                order.TicketsInOrder.AddRange(ticketsInOrder);

                loggedInUser.ShoppingCart.TicketsInShoppingCarts.Clear();
                _userRepository.Update(loggedInUser);
                return true;
            }
            return false;
        }
    }
}
