using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;
using Talabat.Repository;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Extentions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Where(P => P.Value.Errors.Count > 0)
                                                   .SelectMany(P => P.Value.Errors)
                                                   .Select(P => P.ErrorMessage)
                                                   .ToList();
                    var apiValidation = new ApiValidationResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(apiValidation);
                };
            });

            return services;


        }
    }
}
