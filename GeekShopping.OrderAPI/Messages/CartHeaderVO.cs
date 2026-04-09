using System.Text.Json.Serialization;

namespace GeekShopping.OrderAPI.Messages
{
    public class CartHeaderVO
    {
        public long? Id { get; set; }
        public string? UserId { get; set; }

        [JsonPropertyName("CouponCode")]
        public string? CuponCode { get; set; }
    }
}
