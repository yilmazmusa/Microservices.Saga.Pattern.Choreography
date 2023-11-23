using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public class OrderItemMessage 
    {
        public Guid ProductId { get; set; }  // Oluşturulan Siparişin içinde bu üründen vardır.Burda birden fazla ürün olabilir tabiki.
        public int Count { get; set; }   // Oluşturulan Siparişin içinde yukardaki ürünId li üründen şu kadar adet vardır.
        public decimal Price { get; set; } // Oluşturulan Siparişin içinde yukardaki ürünId li ürünün birim fiyatı budur.
    }
}
