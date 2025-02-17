using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Domain.Identity;
using TicketsApp.Repository.Interface;

namespace TicketsApp.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<TicketsAppUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TicketsAppUser>();
        }
        public IEnumerable<TicketsAppUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public TicketsAppUser Get(string id)
        {
            return entities
               .Include(z => z.ShoppingCart)
               .Include("ShoppingCart.TicketsInShoppingCarts")
               .Include("ShoppingCart.TicketsInShoppingCarts.Ticket")
               .Include("ShoppingCart.TicketsInShoppingCarts.Ticket.Movie")
               .Include(z => z.Orders)
               .Include("Orders.TicketsInOrder")
               .Include("Orders.TicketsInOrder.Ticket")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(TicketsAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(TicketsAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(TicketsAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
