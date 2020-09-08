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

                BuildPublishers(channel1, queueName, "Produtor A", "A");
                BuildPublishers(channel2, queueName, "Produtor B", "B");
                BuildPublishers(channel2, queueName, "Produtor C", "C");

                Console.ReadLine();
            }
        }

        public static IModel CreateChannel(IConnection connection)
        {
            var channel = connection.CreateModel();
            return channel;
        }

        public static void BuildPublishers(IModel channel, string queue, string publisherName, string orderSufix)
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
                    string message = $"{publisherName} - OrderNumber: {count++}{orderSufix}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", queue, null, body);

                    Console.WriteLine(message);
                    System.Threading.Thread.Sleep(1000);
                }
            });
        }
    }
}
