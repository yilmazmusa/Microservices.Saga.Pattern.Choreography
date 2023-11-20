
namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            //builder.Services.AddControllers(); //mİCROSERVİCE İLE ÇALIŞACAĞIMIZDAN DOLAYI BURAYI YORUM SATIRI YAPTIK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            
            //if (app.Environment.IsDevelopment()) //BİZ HEP DEVELOPMENT ORTAMINDA ÇALIŞACAĞIMIZ İÇİN GEREK YOK BUNA
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection(); // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ

            //app.UseAuthorization();   // GEREK YOK.LAZIM OLMAYAN MIDDLEWARELERİ KALDIRIYORUZ YANİ


            app.MapControllers();

            app.Run();
        }
    }
}