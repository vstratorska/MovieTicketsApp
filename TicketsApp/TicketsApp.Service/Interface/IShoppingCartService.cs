using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Domain;
using TicketsApp.Domain.Dto;

namespace TicketsApp.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetShoppingCartInfo(string userId);
        bool DeleteTicketFromShoppingCart(string userId, Guid ticketId);
        bool Order(string userId);
        bool AddToShoppingConfirmed(TicketInShoppingCart model, string userId);
    }
}
