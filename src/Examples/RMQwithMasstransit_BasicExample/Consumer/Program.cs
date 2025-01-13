using Consumer;
using MassTransit;

var busControl = Bus.Factory.CreateUsingRabbitMq(busCfg =>
{
    busCfg.ReceiveEndpoint(queueName: "order-created-event-queue", endpointCfg =>
    {
        endpointCfg.Consumer<OrderCreatedConsumer>();
    });
});

await busControl.StartAsync();

Console.ReadKey();
await busControl.StopAsync();