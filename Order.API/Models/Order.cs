using Order.API.Enums;

namespace Order.API.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid BuyerId { get; set; } // Bu BuyerId yi CreateOrderVM tarafında string ile alıyoruz ihtiyaç olursa  içerde Guid'e çaviriyoruz.

        public string? Description { get; set; }
            
        public List<OrderItem>? OrderItems { get; set; }  //Bir Order'ın birdern fazla özelliği olur onları da OrderItem'da tutarız.

        public OrderStatus  OrderStatus{ get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
