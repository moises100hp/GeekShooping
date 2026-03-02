using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/cart";

        public CartService(HttpClient client)
        {
            _client = client;
        }

        public async Task<CartViewModel> FindCartByUserId(string userId)
        {
            var response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");
            return await response.ReadContextAs<CartViewModel>();
        }

        public async Task<CartViewModel> AddItemToCart(CartViewModel cart)
        {
            var response = await _client.PostAsJsonAsync($"{BasePath}/add-cart", cart);
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<CartViewModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<CartViewModel> UpdateCart(CartViewModel cart)
        {
            var response = await _client.PutAsJsonAsync($"{BasePath}/update-cart", cart);
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<CartViewModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> RemoveFromCart(long cartId)
        {
            var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<bool>();

            return false;
        }

        public async Task<bool> ApplyCupon(CartViewModel cart, string cuponCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Checkout(CartHeaderViewModel cartHeader)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCupon(string userId)
        {
            throw new NotImplementedException();
        }

    }
}
