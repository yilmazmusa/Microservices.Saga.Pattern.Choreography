﻿using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent> // OrderCreatedEvent i consum eden bir class burası
    {
        //.NET8 ile gelen primary Constructor ile artık DP yi aşağıdaki gibi Constructor ile gerçekleştirebiliriz
       // ya da 7. satırdaki gibi

        readonly MongoDBService _mongoDBService;
        public OrderCreatedEventConsumer(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();

           IMongoCollection<Models.Stock> stockCollection = _mongoDBService.GetCollection<Models.Stock>();

            foreach (var orderItemMessage in context.Message.OrderItemsMessage)
            {//Clientte gelen ürünId sinden ve miktarından VT var mı kontrolü.Varsa true, yoksa false  ekleyecek boolean tipindeki lsiteye
               stockResult.Add(await (await stockCollection.FindAsync(s => s.ProductId == orderItemMessage.ProductId &&
                                                s.Count >= orderItemMessage.Count)).AnyAsync());
            }

            if (stockResult.TrueForAll(s => s.Equals(true)))
            {//Burda siparişte gelen her ürün(elma,armut,kiraz) için hepsi için true ise çünkü hepsi 1 siparişte geliyo
             //Siparişin gerçekleşmesi için hepsinin true olması lazım yukarda.Hepsi true ise Stock ve Payment işlemleri yapılır.


                //Stock Güncellemesi yapılır
                foreach (var orderItemMessage in context.Message.OrderItemsMessage)
                {//Requestte gelen ve VT bir ProductId ile eşleşen productı aldık stock a attık.Artık bunun Count  bilgisini vs güncelleyebiliriz.
                  Models.Stock product = await  (await stockCollection.FindAsync(s => s.ProductId == orderItemMessage.ProductId)).FirstOrDefaultAsync();

                    product.Count -= orderItemMessage.Count; //Count bilgisini siparişte gelen kadar düşürdük.

                    stockCollection.FindOneAndReplaceAsync(s => s.ProductId == orderItemMessage.ProductId, product);
                
                }

                //Payment.API yi tetikleyecek event yayınlanır

            }

            else
            {//Biri bile false ise(elma,armut,kirazdan) sipariş gerçekleşmez.Yapılcaka tek iş var Order a haber vermmek.
             //Bu sipariş gerçekleşemez bunun durumunu Fail e çek diyeceğiz.

            }
        }
    }
}