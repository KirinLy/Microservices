using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.EventBusConsumer;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
            builder.Services.AddMassTransit(
                config =>
                {
                    config.AddConsumer<BasketCheckoutConsumer1>();
                    config.AddConsumer<BasketCheckoutConsumer2>();

                    config.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(rabbitMqSettings.Uri, cfgHost =>
                        {
                            cfgHost.Username(rabbitMqSettings.UserName);
                            cfgHost.Password(rabbitMqSettings.Password);
                        });
                        cfg.ReceiveEndpoint(rabbitMqSettings.BasketCheckoutQueue1, cfgEndPoint =>
                        {
                            cfgEndPoint.ConfigureConsumer<BasketCheckoutConsumer1>(ctx);
                        });
                        cfg.ReceiveEndpoint(rabbitMqSettings.BasketCheckoutQueue2, cfgEndPoint =>
                        {
                            cfgEndPoint.ConfigureConsumer<BasketCheckoutConsumer2>(ctx);
                        });
                    });
                });  

            builder.Services.AddScoped<IConsumer<BasketCheckoutEvent1>, BasketCheckoutConsumer1>();
            builder.Services.AddScoped<IConsumer<BasketCheckoutEvent>, BasketCheckoutConsumer2>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}