using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                var queueName = "order-to-process";

                var channel1 = CreateChannel(connection);
                var channel2 = CreateChannel(connection);

                BuildPublishers(channel1, queueName, "Produtor A");
                BuildPublishers(channel2, queueName, "Produtor B");
                BuildPublishers(channel2, queueName, "Produtor C");

                Console.ReadLine();
            }
        }

        public static IModel CreateChannel(IConnection connection)
        {
            var channel = connection.CreateModel();
            return channel;
        }

        public static void BuildPublishers(IModel channel, string queue, string publisherName)
        {
            Task.Run(() =>
            {
                var count = 1;

                channel.QueueDeclare(queue: queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                while (true)
                {
                    string message = $"{publisherName} - OrderNumber: {count++}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", queue, null, body);

                    Console.WriteLine($"{publisherName} - [x] Sent {count}", message);
                    System.Threading.Thread.Sleep(1000);
                }
            });
        }
    }
}
