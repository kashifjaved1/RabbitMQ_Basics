using MassTransit;
using Microsoft.EntityFrameworkCore;
using Producer.Data;
using Producer.Services.Contracts;
using Producer.Services.Implementation;

namespace Producer.Extensions
{
    public static class ServicesExtensions
    {
        public static void ProjectConfigurations(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<ApiDbContext>(options => options.UseInMemoryDatabase("rmqMasstransit_DB"));
            services.AddScoped<IOrderService, OrderService>();
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });
        }
    }
}
