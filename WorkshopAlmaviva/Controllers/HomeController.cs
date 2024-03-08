
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WorkshopAlmaviva.Models;

namespace WorkshopAlmaviva.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HttpClient _httpClient;

        private readonly IConfiguration _config;

        List<BookModel> personaList = new List<BookModel>();

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, IConfiguration config)
        {
            _logger = logger;
            _httpClient = httpClient;
            _config = config;
        }

       
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return View("NotLoggedInView");
            }

        }



        public async Task<IActionResult> _bookList(int pageNumber, string word = "")
        {
            string apiUrl = _config.GetValue<string>("UrlConfig:BookListUrl");
            var response = await _httpClient.GetAsync(apiUrl + $"?word={word}&pageNumber={pageNumber}");
            _ = response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var books = JsonConvert.DeserializeObject<PaginatedList<BookModel>>(result);

            if (User.IsInRole("admin"))
            {
                ViewBag.Count = books.Items.Count;
            }
           
                return PartialView(books);

        }

        public async Task<IActionResult> _bookDetails(int id)
        {
            string apiUrl = _config.GetValue<string>("UrlConfig:DetailBookListUrl");
            var response = await _httpClient.GetAsync(apiUrl + id);


            var result = await response.Content.ReadAsStringAsync();
            var book = JsonConvert.DeserializeObject<BookModel>(result);

            return PartialView(book);


        }

        public async Task<IActionResult> _addBook(BookModel book)
        {
            string apiUrl = _config.GetValue<string>("UrlConfig:DefaultswaggerUrl");
            _httpClient.BaseAddress = new Uri(apiUrl);
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Book", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var postResponse = JsonConvert.DeserializeObject<BookModel>(responseContent);

                return RedirectToAction(nameof(Index));

            }
            return PartialView();
        }

        public async Task<IActionResult> _detailsBookUpdate(BookModel book, int id)
        {
            string apiUrl = _config.GetValue<string>("UrlConfig:DetailBookListUrl");
            var response = await _httpClient.GetAsync(apiUrl + id);
            var result = await response.Content.ReadAsStringAsync();

            var persone = JsonConvert.DeserializeObject<BookModel>(result);

            return PartialView(persone);
        }

        public async Task<IActionResult> Update(BookModel book, int id)
        {
            string apiUrl = _config.GetValue<string>("UrlConfig:DefaultswaggerUrl");
            _httpClient.BaseAddress = new Uri(apiUrl);
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/Book/" + id, content);


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            string apiUrl = _config.GetValue<string>("UrlConfig:DetailBookListUrl");
            var response = await _httpClient.DeleteAsync(apiUrl + id);
            var result = await response.Content.ReadAsStringAsync();

            var persone = JsonConvert.DeserializeObject<BookModel>(result);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
