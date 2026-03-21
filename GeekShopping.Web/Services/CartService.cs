using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _client;
        public const string BasePath = "http://localhost:5197/api/v1/cart";

        public CartService(HttpClient client)
        {
            _client = client;
        }

        public async Task<CartViewModel> FindCartByUserId(string userId, string token)
        {
            var response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");
            return await response.ReadContextAs<CartViewModel>();
        }

        public async Task<CartViewModel> AddItemToCart(CartViewModel cart, string token)
        {
            var response = await _client.PostAsJsonAsync($"{BasePath}/add-cart", cart);
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<CartViewModel>();
            else throw new Exception($"Something went wrong when calling API - {response.StatusCode}- {await response.Content.ReadAsStringAsync()}");
        }

        public async Task<CartViewModel> UpdateCart(CartViewModel cart, string token)
        {
            var response = await _client.PutAsJsonAsync($"{BasePath}/update-cart", cart);
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<CartViewModel>();
            else throw new Exception($"Something went wrong when calling API - {response.StatusCode}- {response.Content.ReadAsStringAsync()}");
        }

        public async Task<bool> RemoveFromCart(long cartId, string token)
        {
            var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> ApplyCupon(CartViewModel cart, string token)
        {
            var response = await _client.PostAsJson($"{BasePath}/apply-coupon", cart);
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> RemoveCupon(string userId, string token)
        {
            var response = await _client.DeleteAsync($"{BasePath}/remove-coupon/{userId}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContextAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<CartHeaderViewModel> Checkout(CartHeaderViewModel cartHeader, string token)
        {
            var response = await _client.PostAsJson($"{BasePath}/checkout", cartHeader);
            if(response.IsSuccessStatusCode)
                return await response.ReadContextAs<CartHeaderViewModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public Task<bool> ClearCart(string userId, string token)
        {
            throw new NotImplementedException();
        }
    }
}
