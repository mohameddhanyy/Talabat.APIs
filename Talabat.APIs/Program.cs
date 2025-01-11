using Microsoft.EntityFrameworkCore;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {



            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configre Services
            // Add services to the container.

            webApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });


            #endregion      

            var app = webApplicationBuilder.Build();

            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured when apply update database");
            }
            #region Configre Kestral Middlewares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            #endregion    

            app.Run();
        }
    }
}
