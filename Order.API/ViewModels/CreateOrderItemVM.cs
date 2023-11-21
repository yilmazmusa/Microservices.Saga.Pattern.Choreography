namespace Order.API.ViewModels
{
    public class CreateOrderItemVM
    {
        public string ProductId { get; set; }
        public int Count { get; set; } // Bu ProductId ye karşılık kaç tane sipariş verilmiş.Yani elma ürününden kaç tane sipariş verilmiş gibi
        public decimal Price { get; set; } // Bu ProductId nin Price bilgisi. Yani elma ürününün fiyatı.
    }
}
