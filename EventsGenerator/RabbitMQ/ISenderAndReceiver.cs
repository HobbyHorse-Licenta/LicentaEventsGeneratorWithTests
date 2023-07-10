using EventsGenerator.Entities;

namespace EventsGenerator.RabbitMQ
{
    public interface ISenderAndReceiver
    {
        void ProcessReveivedJson(string json);
        void PublishString(string message);
        void SendPartialAggresiveEvent(Event aggresiveEvent);
        void setupConnectionAndQueue();
    }
}