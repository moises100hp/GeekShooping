using GeekShopping.Web.Models;

public interface ICouponService
{
    Task<CouponViewModel> GetCoupon(string code, string token);
}