using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using EventsGenerator.Entities;
using RabbitMQ.Client.Events;
using Formatting = Newtonsoft.Json.Formatting;
using JsonSerializer = System.Text.Json.JsonSerializer;
using EventsGenerator.EventProcessors;
using EventsGenerator.EventProcessorsInterfaces;

namespace EventsGenerator.RabbitMQ
{
    public class SenderAndReceiver : ISenderAndReceiver
    {
        IConnection _connection;
        IModel _receiveChannel;
        IModel _sendChannel;

        private static readonly string sendQueueName = "receiveForPosting";
        private static readonly string receiveQueueName = "sendForCreation";
        private readonly IAggresiveSkatingController _aggresiveSkatingEventHandler;

        public SenderAndReceiver(IAggresiveSkatingController aggresiveSkatingEventHandler)
        {
            _aggresiveSkatingEventHandler = aggresiveSkatingEventHandler;
            setupConnectionAndQueue();
        }


        public void SendPartialAggresiveEvent(Event aggresiveEvent)
        {
            string json = JsonConvert.SerializeObject(aggresiveEvent, Formatting.Indented);
            PublishString(json);
        }
        public void PublishString(string message)
        {
            Console.WriteLine("SENDING BACK TO THE API THE MESSAGE THROUGH THE QUEUE");
            var body = Encoding.UTF8.GetBytes(message);
            _sendChannel.BasicPublish(exchange: "",
                                 routingKey: sendQueueName,
                                 basicProperties: null,
                                 body: body);
        }

        public void setupConnectionAndQueue()
        {
            _connection = RabbitMQConnection.GetConnection();
            _sendChannel = RabbitMQConnection.GetSenderChannel(sendQueueName);
            _receiveChannel = RabbitMQConnection.GetReceiverChannel(receiveQueueName);

            Console.WriteLine("receive channel ready: " + _receiveChannel.CurrentQueue);
            var consumer = new EventingBasicConsumer(_receiveChannel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("received a message");
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                ProcessReveivedJson(json);
            };

            _receiveChannel.BasicConsume(queue: receiveQueueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public async void ProcessReveivedJson(string json)
        {
            string[] words = json.Split(' ');
            string instructionMessage = words[0];

            switch (instructionMessage)
            {
                case Messages.createAggresiveEvent:
                    Console.WriteLine("Am intrat pe branchul din switch");
                    var aggresiveEventJson = json.Substring($"{Messages.createAggresiveEvent} ".Length);
                    await _aggresiveSkatingEventHandler.createAggresiveEventFromJson(aggresiveEventJson);
                    break;

                case Messages.addScheduleToAggresiveEvents:
                    Console.WriteLine("Am intrat pe branchul de addSchedule to aggresive events");
                    var scheduleJson = json.Substring($"{Messages.addScheduleToAggresiveEvents} ".Length);
                    Schedule schedule = JsonConvert.DeserializeObject<Schedule>(scheduleJson);
                    _aggresiveSkatingEventHandler.addScheduleToExistingEvents(schedule);
                    break;

                default:
                    Console.WriteLine("Default does nothing");
                    break;
            }



        }


    }
}
