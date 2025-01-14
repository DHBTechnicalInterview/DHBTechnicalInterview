using DHBTestApplication.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace DHBTestApplication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // services.AddTransient<ICountryProvider, CountryProvider>();
            return services;
        }
    }
}
