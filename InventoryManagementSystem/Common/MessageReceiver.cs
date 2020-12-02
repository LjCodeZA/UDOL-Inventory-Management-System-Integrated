using InventoryManagementSystem.DAL;
using InventoryManagementSystem.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace InventoryManagementSystem.Common
{
    public class MessageReceiver : DefaultBasicConsumer
    {
        private readonly IModel _channel;
        private IMSContext _dbContext;
        private CommonUse _commonUse;

        public MessageReceiver(IModel channel)
        {
            _channel = channel;
            _dbContext = new IMSContext();
            _commonUse = new CommonUse();
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));
            Console.WriteLine(string.Concat("Routing tag: ", routingKey));

            var readOnlyToBytes = body.ToArray();
            var orderJson = Encoding.UTF8.GetString(readOnlyToBytes);

            orderJson = Encoding.UTF8.GetString(readOnlyToBytes);
            Order deserializedOrder = JsonConvert.DeserializeObject<Order>(orderJson);

            //Convert Order to StockOut
            var stockOutItem = new StockOut();
            stockOutItem.ProductVendorId = deserializedOrder.ProductVendorId;
            stockOutItem.Quantity = deserializedOrder.Quantity;
            stockOutItem.CreatedDate = deserializedOrder.CreatedDate;
            stockOutItem.Recon = null;

            _commonUse.DoStockAllocation(stockOutItem);

            _channel.BasicAck(deliveryTag, false);
        }
    }
}