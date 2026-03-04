using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GeekShopping.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, 
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine($"Diretório de execução: {Directory.GetCurrentDirectory()}");

            var products = await _productService.FindAllProducts();

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.FindProductById(id);   

            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> DetailsPost(ProductViewModel model)
        { 

            CartViewModel cart = new()
            {
                CartHeader = new CartHeaderViewModel
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value,

                }
            };

            return View();
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

        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    


    }
}
