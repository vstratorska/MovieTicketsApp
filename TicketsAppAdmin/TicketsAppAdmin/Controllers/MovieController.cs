using System.Text;
using ExcelDataReader;
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


        public IActionResult CreateMovie([Bind("Id,MovieName,Genres,MovieDescription,MovieImage,Rating")] Movie movie)
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


        public IActionResult EditMovie([Bind("Id,MovieName,Genres,MovieDescription,MovieImage,Rating")] Movie movie)
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

        public IActionResult Import()
        {
            return View();
        }


        public IActionResult ImportMovies(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<Movie> movies = getAllMoviesFromFile(file.FileName);
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/ImportAllMovies";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(movies), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index", "Movie");

        }

        private List<Movie> getAllMoviesFromFile(string fileName)
        {
            List<Movie> movies = new List<Movie>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        movies.Add(new TicketsAppAdmin.Models.Movie
                        {
                            MovieName = reader.GetValue(0).ToString(),
                            Genres = reader.GetValue(1).ToString(),
                            MovieDescription = reader.GetValue(2).ToString(),
                            MovieImage = reader.GetValue(3).ToString(),
                            Rating = (double)reader.GetValue(4)
                        });
                    }

                }
            }
            return movies;

        }

    }
}
