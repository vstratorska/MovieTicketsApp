
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using TicketsAppAdmin.Models;

namespace TicketsAppAdmin.Controllers
{
    
    public class OrderController : Controller
    {
        public OrderController() {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetAllOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }

        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetDetails";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<OrderDto>().Result;


            return View(result);

        }

        public FileContentResult CreateInvoice(string id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:44337/api/Admin/GetOrder";
            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.Owner.Email);

            StringBuilder sb = new StringBuilder();
            double total = 0.0;
            foreach (var item in result.TicketsInOrder)
            {
                sb.AppendLine("Ticket for movie " + item.Ticket.Movie.MovieName + " has quantity " + item.Quantity + " with price " + item.Ticket.Price);
                total += (item.Quantity * item.Ticket.Price);
            }
            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", total.ToString() + "$");

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");

        }

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Orders");
                worksheet.Cell(1, 1).Value = "OrderID";
                worksheet.Cell(1, 2).Value = "Customer UserName";
                worksheet.Cell(1, 3).Value = "Total Price";
                HttpClient client = new HttpClient();
                string URL = "https://localhost:44337/api/Admin/GetAllOrders";

                HttpResponseMessage response = client.GetAsync(URL).Result;
                var data = response.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 0; i < data.Count(); i++)
                {
                    var item = data[i];
                    worksheet.Cell(i + 2, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = item.Owner.Email;
                    double total = 0.0;
                    for (int j = 0; j < item.TicketsInOrder.Count(); j++)
                    {
                        worksheet.Cell(1, 4 + j).Value = "Movie / Price per Ticket / Quantity";
                        worksheet.Cell(i + 2, 4 + j).Value = item.TicketsInOrder.ElementAt(j).Ticket.Movie.MovieName + " / " + item.TicketsInOrder.ElementAt(j).Ticket.Price + " / " + item.TicketsInOrder.ElementAt(j).Quantity;
                        total += (item.TicketsInOrder.ElementAt(j).Quantity * item.TicketsInOrder.ElementAt(j).Ticket.Price);
                    }
                    worksheet.Cell(i + 2, 3).Value = total;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }

        [HttpGet]
        public FileContentResult ExportAllOrdersPDF()
        {

            HttpClient client = new HttpClient();
            string URL = "https://localhost:44337/api/Admin/GetAllOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;

            var document = new DocumentModel();

            var section = new Section(document);
            document.Sections.Add(section);

            for (int i = 0; i < data.Count(); i++)
            {


                var item = data[i];
                section.Blocks.Add(new Paragraph(document, $"Order Number: {item.Id}"));
                section.Blocks.Add(new Paragraph(document, $"Owner Name: {item.Owner.Email}"));

                StringBuilder sb = new StringBuilder();
                double total = 0;
                int quantity = 0;
                foreach (var it in item.TicketsInOrder)
                {
                    total += (it.Quantity * it.Ticket.Price);
                    quantity += it.Quantity;
                }

                section.Blocks.Add(new Paragraph(document, $"Number od tickets in order: {quantity}"));
                section.Blocks.Add(new Paragraph(document, $"Total: {total}"));
                section.Blocks.Add(new Paragraph(document, $" "));
            }

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportAll.pdf");

        }
    }
}
