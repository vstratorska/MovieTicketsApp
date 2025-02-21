using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Domain.Domain;
using TicketsApp.Repository.Interface;

namespace TicketsApp.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            
            if (typeof(T).IsAssignableFrom(typeof(Ticket)))
            {
                return entities
                    .Include("Movie")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(Movie)))
            {
                return entities
                    .Include("Tickets")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(Order)))
            {
                return entities
                    .Include("Owner")
                    .Include("TicketsInOrder")
                    .Include("TicketsInOrder.Ticket")
                    .Include("TicketsInOrder.Ticket.Movie")
                    .AsEnumerable();
            }
            else
            {
                return entities.AsEnumerable();
            }
        }

        public T Get(Guid? id)
        {
            if (typeof(T).IsAssignableFrom(typeof(Ticket)))
            {
                return entities
                    .Include("Movie")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Movie)))
            {
                return entities
                    .Include("Tickets")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Order)))
            {
                return entities
                    .Include("Owner")
                    .Include("TicketsInOrder")
                    .Include("TicketsInOrder.Ticket")
                    .Include("TicketsInOrder.Ticket.Movie")
                    .First(s => s.Id == id);
            }
            else
            {
                return entities.First(s => s.Id == id);
            }
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
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
