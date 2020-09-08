using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

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
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                Console.WriteLine($"{workerName}: [x] Received {message}");
                await ProcessMessage(workerName, message);
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueName, false, consumer);
        }

        public static async Task ProcessMessage(string workerName, string message)
        {
            var msDelay = message.Contains("Produtor A") ? 1000 : message.Contains("Produtor B") ? 2000 : 3000;
            await Task.Delay(msDelay);
            Console.WriteLine($"{workerName}: [x] Processed {message}");
        }
    }
}
