using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }
    }
}
