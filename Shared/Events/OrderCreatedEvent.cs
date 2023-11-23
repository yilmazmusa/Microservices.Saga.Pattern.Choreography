using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; } // Oluşturulan Siparişin Idsi budur.
        public Guid BuyerId { get; set; } // Oluşturulan Siparişin AlıcıId'si budur.

        public decimal TotalPrice { get; set; }  // Oluşturulan Siparişin toplam tutarı budur.
        public List<OrderItemMessage> OrderItemsMessage { get; set; }  // Oluşturulan Siparişin diğer detayları bunun içindedir.
    }
}
