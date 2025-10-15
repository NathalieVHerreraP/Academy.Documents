using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Academy.Documents.Application;

public static class DependecyInyection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(ConfigureAwaitOptions =>
        {
            ConfigureAwaitOptions.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
