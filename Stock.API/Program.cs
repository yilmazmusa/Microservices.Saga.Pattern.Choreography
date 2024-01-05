
using MassTransit;
<<<<<<< Updated upstream
=======
using MongoDB.Driver;
using Shared.QueueNames;
using Stock.API.Consumers;
>>>>>>> Stashed changes
using Stock.API.Services;

namespace Stock.API
{
    public class Program
    {
        public static  void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //// Add services to the container.

            builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(configurator =>
            {
<<<<<<< Updated upstream
=======
                //MassTransit artık bunun Consumer olduğunu biliyor.Yani OrderCreatedEventConsumer un subscribe(abone) olduğu
                //OrderCreatedEvent ilgili kuyruğa(o kuyruğu 31.satırda belirttik) yayınlandığı zaman yakalayıp burayı tetiklemesi gerektiğini biliyor.
                configurator.AddConsumer<OrderCreatedEventConsumer>();
                configurator.AddConsumer<PaymentFailedEventConsumer>();

>>>>>>> Stashed changes
                configurator.UsingRabbitMq((context, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMQ"]);
                });
            });

            builder.Services.AddSingleton<MongoDBService>();

            var app = builder.Build();

            using IServiceScope scope = app.Services.CreateScope();
            MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
            var stockCollection = mongoDBService.GetCollection<Models.Stock>();

            if (!stockCollection.FindSync(session => true).Any())
            {
                 stockCollection.InsertOne(new() { Id = Guid.NewGuid(), ProductId = Guid.NewGuid().ToString(), Count = 150, CreateDate = DateTime.UtcNow });
                 stockCollection.InsertOne(new() { Id = Guid.NewGuid(), ProductId = Guid.NewGuid().ToString(), Count = 20, CreateDate = DateTime.UtcNow });
                 stockCollection.InsertOne(new() { Id = Guid.NewGuid(), ProductId = Guid.NewGuid().ToString(), Count = 50, CreateDate = DateTime.UtcNow });
                 stockCollection.InsertOne(new() { Id = Guid.NewGuid(), ProductId = Guid.NewGuid().ToString(), Count = 15, CreateDate = DateTime.UtcNow });
                 stockCollection.InsertOne(new() { Id = Guid.NewGuid(), ProductId = Guid.NewGuid().ToString(), Count = 5, CreateDate = DateTime.UtcNow });

            }


            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            //app.MapControllers();

            app.Run();
        }
    }
}