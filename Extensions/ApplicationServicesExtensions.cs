using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.APIs.Errors;
using PhoneBook.APIs.Helpers;
using PhoneBook.Core.Repositories;
using PhoneBook.Repository.Repositories;

namespace PhoneBook.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {

                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                                        .SelectMany(P => P.Value.Errors)
                                                        .Select(E => E.ErrorMessage)
                                                        .ToArray();


                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            return services;
        }
    }
}
