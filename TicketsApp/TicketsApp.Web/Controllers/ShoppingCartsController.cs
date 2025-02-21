using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using TicketsApp.Service.Interface;

namespace TicketsApp.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService _shoppingCartService)
        {
            this._shoppingCartService = _shoppingCartService;
        }

        // GET: ShoppingCarts
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = _shoppingCartService.GetShoppingCartInfo(userId);
            return View(dto);
        }


        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.DeleteTicketFromShoppingCart(userId, id);

            return RedirectToAction("Index");

        }


        public IActionResult Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _shoppingCartService.Order(userId);
            return RedirectToAction("Index", "ShoppingCarts");


        }

        public IActionResult SuccessPayment()
        {
            return View();
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            StripeConfiguration.ApiKey = "sk_test_51Io84IHBiOcGzrvu4sxX66rTHq8r5nxIxRiJPbOHB4NwVJOE1jSlxgYe741ITs024uXhtpBFtxm3RoCZc3kafocC00IhvgxkL0";
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shoppingCartService.GetShoppingCartInfo(userId);

            stripeToken = "tok_visa";

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "Tickets Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                this.Order();
                return RedirectToAction("SuccessPayment");

            }
            else
            {
                return RedirectToAction("NotsuccessPayment");
            }
        }

    }
}
