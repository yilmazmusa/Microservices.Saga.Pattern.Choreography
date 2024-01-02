using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockNotReservedEvent // Stock Olmama Durumu, burda Payment e gtmeye gerek yok Sadece Order.API ye gidip bu siparişin durumunu Fail e çekmek yeterli olur
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public string Message { get; set; }
    }
}
