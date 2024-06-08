using MailRegisteration.Presentation.AppContext;
using MailRegisteration.Presentation.MailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MailRegisteration.Presentation.Extention
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer("Server=90.158.106.204,14333; Database=Register; User Id=developer; Password=Ba12345678+; TrustServerCertificate = True;");

            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();


            services.AddScoped<IMailService, MailService>();
            return services;
        }
    }
}
