using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices
{
    public interface ICartService
    {
        Task<CartViewModel> FindCartByUserId(string userId);
        Task<CartViewModel> AddItemToCart(CartViewModel cart);
        Task<CartViewModel> UpdateCart(CartViewModel cart);
        Task<bool> RemoveFromCart(long cartId);

        Task<bool> ApplyCupon(CartViewModel cart, string cuponCode);
        Task<bool> RemoveCupon(string userId);

        Task<bool> ClearCart(string userId);
        Task<bool> Checkout(CartHeaderViewModel cartHeader);
    }
}
