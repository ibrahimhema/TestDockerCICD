using System.Diagnostics;
using DockerTest.Data;
using DockerTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace DockerTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DockerTestContext dockerTestContext;

        public HomeController(ILogger<HomeController> logger,DockerTestContext dockerTestContext)
        {
            _logger = logger;
            this.dockerTestContext = dockerTestContext;
        }

        public IActionResult Index()
        {
            var users=dockerTestContext.Users.ToList();
            return View(users);
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
