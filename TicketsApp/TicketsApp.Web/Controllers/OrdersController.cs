using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TicketsApp.Service.Implementation;
using TicketsApp.Service.Interface;

namespace TicketsApp.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_orderService.GetOrders(userId));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDto = _orderService.GetOrderInfo(id);
            if (orderDto == null)
            {
                return NotFound();
            }

            return View(orderDto);
        }
    }
}
