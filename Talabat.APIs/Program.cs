using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using Talabat.APIs.CustomMiddleware;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // test changes 2

            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configre Services 
            //Add services to the container.
            webApplicationBuilder.Services.AddControllers();
            //.AddNewtonsoftJson(option =>
            //{
            //    option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
            //});

            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            webApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            webApplicationBuilder.Services.AddAppServices();

            webApplicationBuilder.Services.AddIdentityServices(webApplicationBuilder.Configuration);

            webApplicationBuilder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", o =>
                {
                    o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion      

            var app = webApplicationBuilder.Build();

            #region Update Database When Run
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            var _userManager = services.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();
                await StoreContextSeed.Seed(_dbContext);
                await _identityDbContext.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedUserAsync(_userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured when apply update database");
            } 
            #endregion

            #region Configre Kestral Middlewares

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.AddSwaggerMiddlewares();
            }
            app.UseStatusCodePagesWithRedirects("errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.MapControllers();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            #endregion    

            app.Run();
        }
    }
}
