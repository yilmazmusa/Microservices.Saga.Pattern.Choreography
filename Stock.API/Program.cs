
using MassTransit;
using Shared.QueueNames;
using Stock.API.Consumers;
using Stock.API.Services;

namespace Stock.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //// Add services to the container.

            //builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(configurator =>
            {
                //MassTransit artık bunun Consumer olduğunu biliyor.Yani OrderCreatedEventConsumer un subscribe(abone) olduğu
                //OrderCreatedEvent ilgili kuyruğa(o kuyruğu 31.satırda belirttik) yayınlandığı zaman yakalayıp burayı tetiklemesi gerektiğini biliyor.
                configurator.AddConsumer<OrderCreatedEventConsumer>(); 
                configurator.AddConsumer<PaymentFailedEventConsumer>();

                configurator.UsingRabbitMq((context, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMQ"]);
                    _configure.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
                    _configure.ReceiveEndpoint(RabbitMQSettings.Stock_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
                });
            });

            builder.Services.AddSingleton<MongoDBService>();

            var app = builder.Build();

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