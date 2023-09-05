using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security.Authentication.ExtendedProtection;
using System.Timers;
using EventsGenerator.Entities;
using EventsGenerator.RabbitMQ;
using EventsGenerator.EventProcessors;
using EventsGenerator.ExtraNeededClasses;
using Newtonsoft.Json;
using EventsGenerator.Utils;
using EventsGenerator.EventProcessorsInterfaces;

namespace EventsGenerator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private readonly SenderAndReceiver senderAndReceiver = new SenderAndReceiver();
        private readonly ISenderAndReceiver _senderAndReceiver;
        private readonly ICommonProcessor _commonProcessor;
        private readonly ICasualAndSpeedSkating _casualAndSpeedSkating;
        public Worker(ILogger<Worker> logger, ISenderAndReceiver senderAndReceiver,
            ICommonProcessor commonProcessor, ICasualAndSpeedSkating casualAndSpeedSkating)
        {
            _logger = logger;
            _senderAndReceiver = senderAndReceiver;
            _commonProcessor = commonProcessor;
            _casualAndSpeedSkating = casualAndSpeedSkating;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                int intervalInMinutes = 1; //every 1 minute this functions are executed
                Console.WriteLine("Starting maintenance cycle");
                _commonProcessor.DeleteExpiredSchedules();
                _commonProcessor.DeletePassedEvents();

                _casualAndSpeedSkating.GenerateEvents();
                //AggresiveSkatingHandler.updateExistingEventsWithNewPossibleSkaters();
                await Task.Delay(intervalInMinutes * 60 * 1000, stoppingToken);
            }
        }

       
        

    }
}