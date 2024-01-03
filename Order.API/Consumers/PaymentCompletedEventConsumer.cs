using MassTransit;
using Order.API.Models.Context;
using Shared.Events;
using System;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext; // Siparişin durumunu değiştirmek için veritabanına gitmemiz gerekiyor

        public PaymentCompletedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            _orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _orderAPIDbContext.Orders.FindAsync(context.Message.OrderId);
            if (order == null)
                throw new NullReferenceException();

            order.OrderStatus = Enums.OrderStatus.Completed;
            await _orderAPIDbContext.SaveChangesAsync();
        }
    }
}
