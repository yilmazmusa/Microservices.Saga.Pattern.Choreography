using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QueueNames
{
    public static class RabbitMQSettings // Dinlenicek kuyrukları ve isimlerini burda tanımlıyoruz.
    {
        public const string Stock_OrderCreatedEventQueue = "stock_order_created_event_queue";  // Order.API nin fırlatacağı OrderCreatedEvent'i bu kuyruğa atıyo,
                                                                                          // Stock.API de bu kuyruğu dinleyerek hangi üründen ne kadar sipariş edildiğini anlıyo ve haberdar oluyo


    }
}
