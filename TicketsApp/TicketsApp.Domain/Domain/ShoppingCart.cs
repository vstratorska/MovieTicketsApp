using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Identity;

namespace TicketsApp.Domain.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public TicketsAppUser? Owner { get; set; }
        public virtual ICollection<TicketInShoppingCart>? TicketsInShoppingCarts { get; set; }
    }
}
