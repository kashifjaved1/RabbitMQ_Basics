using Core.Commons.Shared;
using MassTransit;
using System.Text.Json;

namespace Consumer
{
    public class OrderCreatedConsumer : IConsumer<IOrderCreated>
    {
        public Task Consume(ConsumeContext<IOrderCreated> context)
        {
            var jsonMessage = JsonSerializer.Serialize(context.Message);
            Console.WriteLine($"Received message: {jsonMessage}");

            return Task.CompletedTask;
        }
    }
}
