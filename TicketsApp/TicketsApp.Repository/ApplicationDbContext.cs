using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Domain;
using TicketsApp.Domain.Domain;
using TicketsApp.Domain.Identity;

namespace TicketsApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<TicketsAppUser>
    {
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketInShoppingCart> TicketsInShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<TicketInOrder> TicketsInOrders { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
    }
}
