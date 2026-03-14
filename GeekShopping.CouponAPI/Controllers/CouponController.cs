using GeekShopping.CouponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository ?? throw new
                ArgumentNullException(nameof(couponRepository));
        }

        [HttpGet("{couponCode}")]
        //[Authorize]
        public async Task<ActionResult> GetCouponByCouponCode(string couponCode)
        {
           var Coupon = await _couponRepository.GetCouponByCouponCode(couponCode);
            if (Coupon is null) return NotFound();
            return Ok(Coupon);
        }
    }
}
