using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using TicketsAppAdmin.Models;

namespace TicketsAppAdmin.Controllers
{
    public class TicketController : Controller
    {

       

        public IActionResult Create()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetAllMovies";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var movies = response.Content.ReadAsAsync<List<Movie>>().Result;
            
            ViewBag.MovieId = new SelectList(movies, "Id", "MovieName");
            return View();
        }

        public IActionResult CreateTicket([Bind("Id,MovieId,Price,Date")] Ticket ticket)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/CreateTicket";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            return RedirectToAction("Details", "Movie", new { id = ticket.MovieId });

        }

        public IActionResult Edit(string id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetTicket";
            var model = new
            {
                Id = id
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(URL, content).Result;
            var result = response.Content.ReadAsAsync<Ticket>().Result;



            HttpClient client2 = new HttpClient();
            string URL2 = "https://localhost:44337/api/Admin/GetAllMovies";
            HttpResponseMessage response2 = client2.GetAsync(URL2).Result;
            var movies = response2.Content.ReadAsAsync<List<Movie>>().Result;
            ViewBag.MovieId = new SelectList(movies, "Id", "MovieName");

            return View(result);
        }


        public IActionResult EditTicket([Bind("Id,MovieId,Price,Date")] Ticket ticket)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/EditTicket";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            return RedirectToAction("Details", "Movie", new { id = ticket.MovieId });

        }

        public IActionResult Delete(string id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetTicket";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Ticket>().Result;

            return View(result);
        }

        public IActionResult DeleteTicket(string id, string movieId)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/DeleteTicket";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            return RedirectToAction("Details", "Movie", new { id = movieId });

        }

    }
}


