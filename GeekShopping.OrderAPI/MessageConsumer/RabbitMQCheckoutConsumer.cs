
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Repository;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly string _hostName = "localhost";
        private readonly string _password = "guest";
        private readonly string _userName = "guest";
        private IConnection _connection;
        private IChannel _channel;
        private IOrderRepository _repository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQCheckoutConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            _connection = await factory.CreateConnectionAsync();

            using var channel = await _connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "checkoutqueue", false, false, false, arguments: null);

            stoppingToken.ThrowIfCancellationRequested();

            _channel = await _connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (chanel, evt) =>
            {
                var context = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckOutHeaderVO vo = JsonSerializer.Deserialize<CheckOutHeaderVO>(context);
                await ProcessOrder(vo);
                await _channel.BasicAckAsync(evt.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync("checkoutqueue", false, consumer);

        }

        private async Task ProcessOrder(CheckOutHeaderVO vo)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                _repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                OrderHeader order = new()
                {
                    UserId = vo.UserId,
                    FirstName = vo.FirstName,
                    LastName = vo.LastName,
                    OrderDetails = new List<OrderDetail>(),
                    CardNumber = vo.CardNumber,
                    CouponCode = vo.CouponCode,
                    CVV = vo.CVV,
                    DiscountAmount = vo.DiscountAmount,
                    Email = vo.Email,
                    ExpiryMonthYear = vo.ExpiryMonthYear,
                    OrderTime = DateTime.Now,
                    PurchaseAmount = vo.PurchaseAmount,
                    PaymentStatus = false,
                    Phone = vo.Phone,
                    Datetime = vo.Datetime
                };

                foreach (var detail in vo.CartDetails)
                {
                    OrderDetail orderDetail = new()
                    {
                        ProductId = detail.ProductId,
                        ProductName = detail.Product.Name,
                        Price = detail.Product.Price,
                        Count = detail.Count,
                    };

                    order.CartTotal += detail.Count;
                    order.OrderDetails.Add(orderDetail);
                }

                await _repository.AddOrder(order);
            }
        }
    }
}
