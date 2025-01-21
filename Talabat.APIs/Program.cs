using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.CustomMiddleware;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            // Start Working in Session 3
            // Start Working in Session 4 v 2

            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configre Services 
            //Add services to the container.
            webApplicationBuilder.Services.AddControllers();

            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            webApplicationBuilder.Services.AddAppServices();
            #endregion      

            var app = webApplicationBuilder.Build();

            #region Update Database When Run
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();
                await DataSeedingContext.Seed(_dbContext);
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

            app.UseAuthentication();

            app.UseAuthorization();

            #endregion    

            app.Run();
        }
    }
}
