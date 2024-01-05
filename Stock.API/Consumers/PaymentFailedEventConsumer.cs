using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        MongoDBService _mongoDBService;

        public PaymentFailedEventConsumer(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var stockCollection = _mongoDBService.GetCollection<Models.Stock>();//mongoDB de tutulan tüm bilgileri Count(stock) bilgilerini çektik

            foreach (var orderItem in context.Message.OrderItems)
            {
                var product = await ((await stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync());

                if (product != null)
                {
                    product.Count += orderItem.Count; // Ödemesi başarısız olan ürünün count(stok) bilgisini tekrar eski haline getirdik.Çünkü ürün satılmadı ödeme başarısız oldu
                    stockCollection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId.ToString(), product); // Güncelleme işlemini VERİTABANINA yansıttık.(Client tarafından siparişte istenilen ama ödemesi başarısız olan product ın count bilgisini güncelle dedik.)
                }
              
            }
        }
    }
}
