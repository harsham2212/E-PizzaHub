using epizzahub.Entitites;
using epizzahub.Entitites.Entitites;
using epizzahub.Repositories.Implemenations;
using epizzahub.Repositories.Interfaces;
using epizzahub.Services.Implementation;
using epizzahub.Services.Interfaces;
using ePizzaHub.Repositories.Implementations;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services
{
    public class ConfigureDependencies
    {
        public static void RegisterService(IServiceCollection services,IConfiguration configuration)
        {
            //database
            services.AddDbContext<AppDBContext>(Options => Options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IRepostiory<Item>, Repository<Item>>();
            services.AddScoped<IRepostiory<CartItem>, Repository<CartItem>>();
            services.AddScoped<IRepostiory<PaymentDetail>, Repository<PaymentDetail>>();

            //services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();

        }
    }
}
