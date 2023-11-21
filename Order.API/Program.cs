
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Context;
using Order.API.ViewModels;
using Order.API.Enums;

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

            builder.Services.AddMassTransit(configurator =>
            {
                configurator.UsingRabbitMq((context, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMQ"]);
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

            app.MapPost("/create/order", async (CreateOrderVM model, OrderAPIDbContext context) =>
            {
                Order.API.Models.Order order = new()
                {
                    BuyerId = Guid.TryParse(model.BuyerId, out Guid _buyerId) ? _buyerId : Guid.NewGuid(), //Client'ten gelen veri Guid'e çevrilebiliyorsa çevir ve ata, çevrilemiyorsa yeni bir Guid oluştur ve ata.
                    OrderItems = model.OrderItems.Select(oi => new Order.API.Models.OrderItem()
                    {

                        ProductId = Guid.TryParse(oi.ProductId, out Guid _productId) ? _productId : Guid.NewGuid(),
                        Count = oi.Count,
                        Price = oi.Price
                    }).ToList(), // Yukarda tip dönüşümü yaparken Select yaptık Select bize IEnumarable döner ama biz List bekliyoruz çünkü OrderItems List türünden o yüzden ToList() dedik.                               
                    OrderStatus = OrderStatus.Suspend,
                    CreatedDate = DateTime.UtcNow,
                    TotalPrice = model.OrderItems.Sum(oi => oi.Price * oi.Count), // Mesela 3 tane 6000 tl lik koltuk sipariş ettiğinde gibi

                };
                await context.Orders.AddAsync(order); // Yukarda requestle gelen bilgileri Orders tablosuna ekledik AMA eklemeden önce
                                                      // inject etmemiz gerekiyordu onu da yukarda OrderAPIDbContext context diyerek
                                                      // inject etmiş olduk.Sonra o inject ettiğimiz nesneyle gittik VT ekledik.

                await context.SaveChangesAsync(); // Ekleme işlemini VT nına yansıttık.
            });



            app.UseHttpsRedirection(); // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ

            app.UseAuthorization();   // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ


            app.MapControllers();

            app.Run();
        }
    }
}