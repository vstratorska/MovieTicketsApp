using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketsAppAdmin.Models;

namespace TicketsAppAdmin.Controllers
{
    public class MovieController : Controller
    {

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetAllMovies";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Movie>>().Result;
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetMovie";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Movie>().Result;

            return View(result);
        }


        public IActionResult CreateMovie([Bind("Id,MovieName,MovieDescription,MovieImage,Rating")] Movie movie)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/CreateMovie";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            return RedirectToAction("Index", "Movie");

        }

        public IActionResult Edit(string id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetMovie";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Movie>().Result;

            return View(result);
        }


        public IActionResult EditMovie([Bind("Id,MovieName,MovieDescription,MovieImage,Rating")] Movie movie)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/EditMovie";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            return RedirectToAction("Index", "Movie");

        }

        public IActionResult Delete(string id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetMovie";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Movie>().Result;

            return View(result);
        }

        public IActionResult DeleteMovie(string id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/DeleteMovie";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            return RedirectToAction("Index", "Movie");

        }

    }
}
