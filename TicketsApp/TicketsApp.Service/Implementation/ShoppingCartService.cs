using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
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
        //private readonly IEmailService _emailService;


        public ShoppingCartService(IRepository<TicketInOrder> ticketInOrderRepository, IRepository<Order> orderRepository, IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository)
        {
            this._ticketInOrderRepository = ticketInOrderRepository;
            this._orderRepository = orderRepository;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
            //_emailService = emailService;
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
                //EmailMessage message = new EmailMessage();
                //message.Subject = "Successfull order";
                //message.MailTo = loggedInUser.Email;

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


                //StringBuilder sb = new StringBuilder();
                //var totalPrice = 0.0;
                //sb.AppendLine("Your order is completed. The order conatins: ");
                //for (int i = 1; i <= list.Count(); i++)
                //{
                //    var currentItem = list[i - 1];
                //    totalPrice += currentItem.Quantity * currentItem.Ticket.Price;
                //    sb.AppendLine(i.ToString() + ". " + currentItem.Ticket.Movie.MovieName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Ticket.Price);
                //}
                //sb.AppendLine("Total price for your order: " + totalPrice.ToString());
                //message.Content = sb.ToString();

                               

                foreach (var ticket in ticketsInOrder)
                {
                    _ticketInOrderRepository.Insert(ticket);
                }

                order.TicketsInOrder.AddRange(ticketsInOrder);

                loggedInUser.ShoppingCart.TicketsInShoppingCarts.Clear();
                _userRepository.Update(loggedInUser);
                //this._emailService.SendEmailAsync(message);

                return true;
            }
            return false;
        }
    }
}
