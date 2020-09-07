using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;

namespace consumer
{
    class Program
    {
        static string queueName = "order-to-process";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                BuildAndRunWorker(channel, "Worker 1");
                BuildAndRunWorker(channel, "Worker 2");

                Console.ReadLine();
            }
        }

        public static void BuildAndRunWorker(IModel channel, string workerName)
        {
            channel.BasicQos(0, 1, false);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                Console.WriteLine($"{workerName}: [x] Received {message}");

                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueName, false, consumer);
        }
    }
}
