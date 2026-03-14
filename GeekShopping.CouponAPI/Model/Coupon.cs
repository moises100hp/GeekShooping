using GeekShopping.CouponAPI.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CouponAPI.Model
{
    [Table("coupon")]
    public class Coupon : BaseEntity
    {
        [Column("cupon_code")]
        [Required]
        [StringLength(150)]
        public string CuponCode { get; set; }

        [Column("discount_amouth")]
        [Required]
        public decimal DiscountAmouth { get; set; }
    }
}
