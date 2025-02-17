using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Domain;
using TicketsApp.Repository.Interface;
using TicketsApp.Service.Interface;

namespace TicketsApp.Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Movie> _movietRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(IRepository<Ticket> ticketRepository,IRepository<Movie> movieRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _movietRepository = movieRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
            _userRepository = userRepository;
        }

        public void CreateNewTicket(Ticket t)
        {    
            var movie = _movietRepository.Get(t.MovieId);
            t.Movie = movie;

            _ticketRepository.Insert(t);

            movie.Tickets.Add(t);
            _movietRepository.Update(movie);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = _ticketRepository.Get(id);
            //var movie = _movietRepository.Get(ticket.MovieId);
            //movie.Tickets.Remove(ticket);
            //_movietRepository.Update(movie);
            _ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            var ticket = _ticketRepository.Get(id);
            return ticket;
        }

        public void UpdateExistingTicket(Ticket t)
        {
            _ticketRepository.Update(t);
        }
    }
}
