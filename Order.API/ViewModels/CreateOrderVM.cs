namespace Order.API.ViewModels
{
    public class CreateOrderVM // kULLANICIDAN GELEN BÜTÜN VERİLERİ BUNUNLA KARŞILAYACAĞIZ.
    {
        public string BuyerId { get; set; } // Normalde Gelen istekte BuyerId'yi Guid ile karşılayama çalışırsak bu hataya yol açabilir.
                                            // O yüzden VM de bu değeri string değer ile karşılarız içerde ihtiyacımız olan yerde Guid'e çeviririz.
                                            
        public List<CreateOrderItemVM> OrderItems { get; set; } // Bir Order'ın birdern fazla özelliği olur onları da OrderItem'da tutarız.
    }
}
