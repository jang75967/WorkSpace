using Microsoft.AspNetCore.Mvc;
using MVCTest.Models;
using System.Diagnostics;
using System.Text.Json;

namespace MVCTest.Controllers
{

    // Controller 제외한 앞부분을 url로 사용
    // localhost:port/home
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // DI 사용
        // 생성자에서 인터페이스를 파라미터로 지정했을 때, 인터페이스가 asp.net 프레임워크에 등록되어 있으면 객체 생성해서 전달
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        // localhost:port/home/index
        public IActionResult Index()
        {
            return View();
        }

        // localhost:port/home/privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // localhost:port/home/authorList
        public IActionResult AuthorList()
        {
            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "authors.txt");
            string json = System.IO.File.ReadAllText(path);

            var authors = JsonSerializer.Deserialize<List<Author>>(json);

            return View(authors);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}