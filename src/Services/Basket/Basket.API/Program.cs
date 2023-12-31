
using Basket.API.Repositories;
using Basket.API.Services;
using Discount.Grpc.Protos;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API
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
            builder.Services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = builder.Configuration.GetValue<string>("Cache:ConnectionString");
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddAutoMapper(typeof(Program));


            builder.Services.AddGrpcClient<CouponProtoService.CouponProtoServiceClient>
                (o => o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")));

            var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
            builder.Services.AddMassTransit(
                config =>
                {
                    config.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(rabbitMqSettings.Uri, cfg =>
                        {
                            cfg.Username(rabbitMqSettings.UserName);
                            cfg.Password(rabbitMqSettings.Password);
                        });
                    });
                });


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