using Academy.Documents.Infrastructure.InyectionManagers;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Academy.Documents.Infrastructure;

public static class DependecyInyection
{
    public static IServiceCollection AddInfrastructureService (this IServiceCollection services)
    {
        RepositoryManager.AddRepositories(services);
        return services;
    }
}
