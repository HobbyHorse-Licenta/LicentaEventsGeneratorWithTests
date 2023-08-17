using RabbitMQ.Client;
using System.Security.Policy;
using System.Threading.Channels;

namespace EventsGenerator.RabbitMQ
{
    public class RabbitMQConnection
    {
        private static readonly object lockObject = new object();
        private static IConnection connection;
        private static IModel _receiveChannel;
        private static IModel _sendChannel;
        private static readonly string _url = "amqps://xgwmumwf:L-gk4f_j6E-607ZIFl4p01t2_GOceWsJ@rat.rmq2.cloudamqp.com/xgwmumwf";
        private RabbitMQConnection() { }

        public static IConnection GetConnection()
        {
            if (connection == null)
            {
                lock (lockObject)
                {
                    if (connection == null)
                    {
                        var factory = new ConnectionFactory() { Uri = new Uri(_url) };
                        connection = factory.CreateConnection();
                    }
                }
            }

            return connection;
        }
        public static IModel GetSenderChannel(string sendQueueName)
        {
            if (connection != null && _sendChannel == null)
            {
                lock (lockObject)
                {
                    if (connection != null && _sendChannel == null)
                    {
                        _sendChannel = connection.CreateModel();
                        _sendChannel.QueueDeclare(queue: sendQueueName,
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                    }
                }
            }

            return _sendChannel;
        }

        public static IModel GetReceiverChannel(string receiveQueueName)
        {
            if (connection != null && _receiveChannel == null)
            {
                lock (lockObject)
                {
                    if (connection != null && _receiveChannel == null)
                    {
                        _receiveChannel = connection.CreateModel();
                        _receiveChannel.QueueDeclare(queue: receiveQueueName,
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                    }
                }
            }

            return _receiveChannel;
        }

    }
}
