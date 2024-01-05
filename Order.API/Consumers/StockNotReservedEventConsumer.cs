using MassTransit;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers
{
    public  class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext; // Stock yoksa ilgili siparişin durumunu VT da Fail e çekmek için VT kullanmamız gerekli


        public StockNotReservedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            _orderAPIDbContext = orderAPIDbContext;
        }


        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _orderAPIDbContext.Orders.FindAsync(context.Message.OrderId); // Stoğu olmadığı için Fail olan siparişi VT nında buluyoruz.

            if (order == null)
                throw new NullReferenceException();

            order.OrderStatus = Enums.OrderStatus.Fail; //o siparişin durumunu Fail'e çekiyoruz
            await _orderAPIDbContext.SaveChangesAsync(); // durum değişikliğini VT ına yansıtıyoruz.
        }
    }
}
