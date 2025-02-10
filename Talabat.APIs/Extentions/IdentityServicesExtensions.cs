using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Identity;
using Talabat.Service;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Talabat.APIs.Extentions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                       ValidateAudience = true,
                       ValidAudience = configuration["JWT:ValidAudience"],
                       ValidateIssuer = true,
                       ValidIssuer= configuration["JWT:ValidIssure"],
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDays"]))
                    };
                });
            return services;
        }
    }
}
