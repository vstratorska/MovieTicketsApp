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

    }
}
