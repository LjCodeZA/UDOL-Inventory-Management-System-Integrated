using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagementSystem.Common
{
    public class RabbitMQConnector
    {
        static IConnection conn = null;
        static IModel channel = null;

        public RabbitMQConnector()
        {
            OpenConnection();
        }

        public void ConsumeMessages()
        {
            // accept only one unack-ed message at a time

            // uint prefetchSize, ushort prefetchCount, bool global

            channel.BasicQos(0, 1, false);

            MessageReceiver messageReceiver = new MessageReceiver(channel);

            channel.BasicConsume(Properties.Settings.Default.RabbitMQQueue, false, messageReceiver);
        }

        private void OpenConnection()
        {
            string user = Properties.Settings.Default.RabbitMQUser;
            string pass = Properties.Settings.Default.RabbitMQPassword;
            string vhost = Properties.Settings.Default.RabbitMQVhost;

            ConnectionFactory factory = new ConnectionFactory();

            factory.UserName = user;
            factory.Password = pass;
            factory.VirtualHost = vhost;

            factory.Uri = new Uri(Properties.Settings.Default.RabbitMQAMQPS);
            conn = factory.CreateConnection();

            channel = conn.CreateModel();
        }

        public void CloseConnection()
        {
            channel.Close();
            conn.Close();
        }
    }
}