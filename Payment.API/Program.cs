using MassTransit;
using Shared.Events;

namespace Payment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddMassTransit(configurator =>
            {
                configurator.UsingRabbitMq((context, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMQ"]);
                });
            });

            // Add services to the container.
            //builder.Services.AddControllersWithViews();

            var app = builder.Build();

            //Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection(); // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute( // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            //CONTROLLER değil Minimall apileri kullandığımız için yukardaki bazı yerleri yorum satırına aldık.
        }
    }
}