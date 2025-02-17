using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketsApp.Domain.Domain;

namespace TicketsApp.Domain.Identity
{
    public class TicketsAppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
