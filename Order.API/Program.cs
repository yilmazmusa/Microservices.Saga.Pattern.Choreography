
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Context;
using Order.API.ViewModels;
using Order.API.Enums;
using Shared.Events;
using Shared.Messages;
using Order.API.Consumers;
using Shared.QueueNames;
using Payment.API.Consumers;

using Order.API.Consumers;
using Shared.QueueNames;


namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers(); //mİCROSERVİCE İLE ÇALIŞACAĞIMIZDAN DOLAYI BURAYI YORUM SATIRI YAPTIK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(configurator =>  // Burda MassTransit.RabbitMQ yu kurduk
            {

                configurator.UsingRabbitMq((context, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMQ"]);

                configurator.AddConsumer<PaymentCompletedEventConsumer>();
                configurator.AddConsumer<PaymentFailedEventConsumer>();
                configurator.AddConsumer<StockReservedEventConsumer>();
                configurator.AddConsumer<StockNotReservedEventConsumer>();
                configurator.UsingRabbitMq((context, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMQ"]);
                    _configure.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(context)); // Burda kuyruk bu o kuyruğuda dinleyen Consumer bu diyoruz.
                    _configure.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
                    _configure.ReceiveEndpoint(RabbitMQSettings.Payment_StockReservedEventQueue, e => e.ConfigureConsumer<StockReservedEventConsumer>(context));
                    _configure.ReceiveEndpoint(RabbitMQSettings.Order_StockNotReservedEventQueue, e => e.ConfigureConsumer<StockNotReservedEventConsumer>(context));
                


                });
                
            });

            builder.Services.AddDbContext<OrderAPIDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("MSSQLServer")));



            var app = builder.Build();


            //if (app.Environment.IsDevelopment()) //BİZ HEP DEVELOPMENT ORTAMINDA ÇALIŞACAĞIMIZ İÇİN GEREK YOK BUNA
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.MapPost("/create/order", async (CreateOrderVM model, OrderAPIDbContext context, IPublishEndpoint publishEndpoint) =>
            {
                Order.API.Models.Order order = new()
                {
                    BuyerId = Guid.TryParse(model.BuyerId, out Guid _buyerId) ? _buyerId : Guid.NewGuid(), //Client'ten gelen veri Guid'e çevrilebiliyorsa çevir Guid tipinde _buyerId değerine ata onu da BuyerId ye ata, çevrilemiyorsa yeni bir Guid oluştur ve ata.
                    OrderItems = model.OrderItems.Select(oi => new Order.API.Models.OrderItem()
                    {

                        ProductId = Guid.TryParse(oi.ProductId, out Guid _productId) ? _productId : Guid.NewGuid(),
                        Count = oi.Count,
                        Price = oi.Price

                    }).ToList(), // Yukarda tip dönüşümü yaparken Select yaptık Select bize IEnumarable döner ama biz List bekliyoruz çünkü OrderItems List türünden o yüzden ToList() dedik.                               
                    OrderStatus = OrderStatus.Suspend,
                    CreatedDate = DateTime.Now,
                    TotalPrice = model.OrderItems.Sum(oi => oi.Price * oi.Count), // Mesela 3 tane 6000 tl lik koltuk sipariş ettiğinde gibi

                };
                await context.Orders.AddAsync(order); // Yukarda requestle gelen bilgileri Orders tablosuna ekledik AMA eklemeden önce
                                                      // inject etmemiz gerekiyordu onu da yukarda OrderAPIDbContext context diyerek
                                                      // inject etmiş olduk.Sonra o inject ettiğimiz nesneyle gittik VT ekledik.

                await context.SaveChangesAsync(); // Ekleme işlemini VT nına yansıttık.

                OrderCreatedEvent orderCreatedEvent = new()
                {
                    OrderId = order.Id,
                    BuyerId = order.BuyerId,// Burda VM danda model.BuyerId şekilde alabilirdik.Zaten VM ile Clientten alınıp ana modele atanıyor.
                    TotalPrice = order.TotalPrice,
                    OrderItemsMessage = order.OrderItems.Select(oi => new OrderItemMessage()
                    {
                        ProductId = oi.ProductId,
                        Count = oi.Count,
                        Price = oi.Price
                    }).ToList()
                };

                await publishEndpoint.Publish(orderCreatedEvent); // Oluşturduğumuz Eventi(orderCreatedEvent) Publish ediyoruz yani yayınlıyoruz.
            });



            app.UseHttpsRedirection(); // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ

            app.UseAuthorization();   // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ


            app.MapControllers();

            app.Run();
        }
    }
}

