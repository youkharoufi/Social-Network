using Facebook.Data;
using Facebook.Interface;
using Facebook.Repositories;
using Facebook.Token;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Extrensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
           IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                object value = opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                opt.EnableSensitiveDataLogging();
            });

            services.AddCors();

            //token :
            services.AddScoped<ITokenService, TokenService>();


            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddScoped<IFriendsRepository, FriendsRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            return services;


        }
    }
}
