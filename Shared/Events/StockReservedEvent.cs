using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockReservedEvent //Stock olma durumunda Ödeme için 
    {
        public Guid BuyerId { get; set; }  //Şu kişiden
        public Guid OrderId { get; set; } // Şu sipariş için 
        public decimal TotalPrice { get; set; } // Şu kadar ücret
        public List<OrderItemMessage> OrderItemMessage { get; set; } // Sipariş detayları hangi üründen ne kadar vs aldı

    }
}
