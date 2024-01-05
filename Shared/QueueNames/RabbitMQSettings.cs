using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QueueNames
{
    public static class RabbitMQSettings // Dinlenicek kuyrukları ve isimlerini burda tanımlıyoruz.
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";  // Order.API nin fırlatacağı OrderCreatedEvent'i bu kuyruğa atıyo,
                                                                                               // Stock.API de bu kuyruğu dinleyerek hangi üründen ne kadar sipariş edildiğini anlıyo ve haberdar oluyo

        public const string Payment_StockReservedEventQueue = "payment_stock-reserved_event_queue"; //Payment.API nin dinleyeceği  stock var durumu için

        public const string Order_StockNotReservedEventQueue = "order_stock-not-reserved_event_queue"; //Order.API nin dinleyeceği  stock yok durumu için

        public const string Order_PaymentCompletedEventQueue = "order-payment-completed-event-queue"; //Order.API nin dinleyeceği ödeme başarılı durumu için

        public const string Order_PaymentFailedEventQueue = "order-payment-failed-event-queue";  //Order.API nin dinleyeceği ödeme başarısız durumu için

        public const string Stock_PaymentFailedEventQueue = "stock-payment-failed-event-queue"; //Stock.API nin dinleyeceği  ödeme başarısız durumu için.Yapılan stok işlemlerini geri almak için

        
    }
}
