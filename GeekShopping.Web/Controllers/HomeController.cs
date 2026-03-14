using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeekShopping.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, 
            IProductService productService,
            ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine($"Diretório de execução: {Directory.GetCurrentDirectory()}");

            var token = await HttpContext.GetTokenAsync("access_token");

            var products = await _productService.FindAllProducts(token);

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var product = await _productService.FindProductById(id, token);   

            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> DetailsPost(ProductViewModel model)
        {

            var token = await HttpContext.GetTokenAsync("access_token");

            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            CartViewModel cart = new()
            {
                CartHeader = new CartHeaderViewModel
                {
                    UserId = UserId ?? "",
                    CouponCode = ""
                }
            };

            CartDetailViewModel cartDetail = new()
            {
                Count = model.Count,
                ProductId = model.Id,
                Product = await _productService.FindProductById(model.Id, token),
                CartHeader = cart.CartHeader,
            };

            List<CartDetailViewModel> cartDetailViewModels = [cartDetail];
            cart.CartDetails = cartDetailViewModels;

            var response = await _cartService.AddItemToCart(cart, token);

            if (response != null)
                return RedirectToAction(nameof(Index));

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
