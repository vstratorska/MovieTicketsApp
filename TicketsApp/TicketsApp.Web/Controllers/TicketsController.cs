using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Domain.Domain;
using TicketsApp.Service.Implementation;
using TicketsApp.Service.Interface;

namespace TicketsApp.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IMovieService _movieService;
        private readonly IShoppingCartService _shoppingCartService;

        public TicketsController(ITicketService ticketService, IMovieService movieService, IShoppingCartService shoppingCartService)
        {
            _ticketService = ticketService;
            _movieService = movieService;
            _shoppingCartService = shoppingCartService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            return View(_ticketService.GetAllTickets());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            var movies = _movieService.GetAllMovies();
            ViewBag.MovieId = new SelectList(movies, "Id", "MovieName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,MovieId,Price,Date")] Ticket ticket)
        {
           
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                _ticketService.CreateNewTicket(ticket);
                return RedirectToAction("Details", "Movies", new { id = ticket.MovieId });

            }
            return View(ticket);
        }

        public IActionResult AddToCart(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id);

            TicketInShoppingCart ts = new TicketInShoppingCart();

            if (ticket != null)
            {
                ts.TicketId = ticket.Id;
            }

            return View(ts);
        }

        [HttpPost]
        public IActionResult AddToCartConfirmed(TicketInShoppingCart model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.AddToShoppingConfirmed(model, userId);

            return RedirectToAction("Index", "ShoppingCarts");
        }


        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var movies = _movieService.GetAllMovies();
            ViewBag.MovieId = new SelectList(movies, "Id", "MovieName");
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,MovieId,Price,Date")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ticketService.UpdateExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Details", "Movies", new { id = ticket.MovieId });

            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var ticket = _ticketService.GetDetailsForTicket(id);
            var movieId = ticket.MovieId;
            _ticketService.DeleteTicket(id);
            return RedirectToAction("Details", "Movies", new { id = movieId });

        }
    }
}
