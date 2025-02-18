using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TicketsApp.Domain.Domain;
using TicketsApp.Domain.Dto;
using TicketsApp.Service.Interface;

namespace TicketsApp.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMovieService _movieService;
        private readonly ITicketService _ticketService;

        public AdminController(IOrderService orderService, IMovieService movieService, ITicketService ticketService)
        {
            _orderService = orderService;
            _movieService = movieService;
            _ticketService = ticketService;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllOrders()
        {
            return this._orderService.GetAllOrders();
        }

        [HttpGet("[action]")]
        public List<Movie> GetAllMovies()
        {
            return this._movieService.GetAllMovies();
        }

        [HttpPost("[action]")]
        public OrderDto GetDetails(BaseEntity id)
        {
            return this._orderService.GetOrderInfo(id.Id);
        }

        [HttpPost("[action]")]
        public Movie GetMovie(BaseEntity id)
        {
            return _movieService.GetDetailsForMovie(id.Id);
        }

        [HttpPost("[action]")]
        public void CreateMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Id = Guid.NewGuid();
                this._movieService.CreateNewMovie(movie);
                
            }
        }

        [HttpPost("[action]")]
        public void EditMovie(Movie movie)
        {
           if (ModelState.IsValid)
            {
                _movieService.UpdateExistingMovie(movie);
            }
        }

        [HttpPost("[action]")]
        public void DeleteMovie(BaseEntity id)
        {
            if (ModelState.IsValid)
            {
                _movieService.DeleteMovie(id.Id);
            }
        }


        [HttpPost("[action]")]
        public Ticket GetTicket(BaseEntity id)
        {
            return _ticketService.GetDetailsForTicket(id.Id);
        }

        [HttpPost("[action]")]
        public void CreateTicket(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                this._ticketService.CreateNewTicket(ticket);

            }
        }

        [HttpPost("[action]")]
        public void EditTicket(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _ticketService.UpdateExistingTicket(ticket);
            }
        }

        [HttpPost("[action]")]
        public void DeleteTicket(BaseEntity id)
        {
            if (ModelState.IsValid)
            {
                _ticketService.DeleteTicket(id.Id);
            }
        }
    }
}
