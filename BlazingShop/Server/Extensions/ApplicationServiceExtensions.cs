using BlazingShop.Server.DataBase;
using BlazingShop.Server.DataBase.Operations.PaymentService;
using BlazingShop.Server.DataBase.Operations.CategoryServiceDB;
using BlazingShop.Server.DataBase.Operations.ProductServiceDB;
using BlazingShop.Server.DataBase.Operations.StatsServiceDB;
using BlazingShop.Server.DataBase.Operations.TokenService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BlazingShop.Server.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=blazerShop-db;Trusted_Connection=True;"));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStatsService, StatsService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
