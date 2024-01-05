using MassTransit;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext;

        public PaymentFailedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            _orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _orderAPIDbContext.Orders.FindAsync(context.Message.OrderId);
            if (order == null)
                throw  new NullReferenceException();

            order.OrderStatus = Enums.OrderStatus.Fail;
            await _orderAPIDbContext.SaveChangesAsync();
        
        
        }
    }
}
