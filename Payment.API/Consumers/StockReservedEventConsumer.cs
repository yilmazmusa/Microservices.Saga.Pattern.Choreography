using MassTransit;
using Shared.Events;
using Shared.QueueNames;

namespace Payment.API.Consumer
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;
        readonly ISendEndpointProvider _sendEndpointProvider;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (true)
            {//Ödeme Başarılıysa(Bankaya vs gitme işlemlerini yapmamak için direk başarılıymış gibi yaptık)

                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Ödeme işlemi başarılı"

                };

                // Başarılı olma durumunda sadece Order.API ye event göndereceğimiz için bu eventi ISendEndpointProvider ile gönderdik.
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue: {RabbitMQSettings.Order_PaymentCompletedEventQueue}")); // Eventin gönderileceği kuyruğu oluşturduk

                await sendEndpoint.Send(paymentCompletedEvent);
                await Console.Out.WriteLineAsync("Ödeme başarılı");


            }
            else
            { //Ödeme başarısızsa.Hem Order.API ye sipariş başarısız eventi gitsin sipariş durumu Fail'e çekilsein
              //hemde Stock.API ye event gitsin ki stoktan düşen ürün adedi geri alınsın(Compansable Transaction) çünkü satış olmadı 
              //Bu iki yerde olan geri alma işlemine Kareografi davranışıyla ilerliyoruz.

               

                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Ödeme esnasında bir sorun oluştu."
                };

                // Başarısız olma durumunda hem Stock.API ye hemde Order.API ye durum ile ilgili event göndereceğimiz için IPublishEndpoint ile gönderdik.
                await _publishEndpoint.Publish(paymentFailedEvent);
                await Console.Out.WriteLineAsync("Ödeme başarısız.");

            }
        }
    }
}
