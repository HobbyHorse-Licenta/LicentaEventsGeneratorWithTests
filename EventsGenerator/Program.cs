using EventsGenerator;
using EventsGenerator.EventProcessors;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.RabbitMQ;
using EventsGenerator.Utils;
using EventsGenerator.UtilsInterfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ISenderAndReceiver,SenderAndReceiver>();

        services.AddSingleton<IFetch, Fetch>();
        services.AddSingleton<IProcessingUtils, ProcessingUtils>();
        services.AddSingleton<IAggresiveSkatingEventProcessor, AggresiveSkatingEventProcessor>();
        services.AddSingleton<IAggresiveSkatingController, AggresiveSkatingController>();
        services.AddSingleton<IAggresiveSkating, AggresiveSkating>();
        services.AddSingleton<ICommonProcessor, CommonProcessor>();
        services.AddSingleton<ICasualAndSpeedSkating, CasualAndSpeedSkating>();
        services.AddSingleton<ICasualAndSpeedSkatingPairingsFinder, CasualAndSpeedSkatingPairingsFinder>();
        services.AddSingleton<ICasualAndSpeedSkatingEventGenerator, CasualAndSpeedSkatingEventGenerator>();

    })
    .Build();

await host.RunAsync();
