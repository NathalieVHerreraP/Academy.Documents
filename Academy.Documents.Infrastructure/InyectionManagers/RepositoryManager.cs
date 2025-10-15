using Academy.Documents.Domain.Entities.DocumentsEntity.Repositories;
using Academy.Documents.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Academy.Documents.Infrastructure.InyectionManagers;

public static class RepositoryManager
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IDocumentsRepository, DocumentsRepository>();
        services.AddTransient<IVirusTotalApi, VirusTotalApi>();

        return services;
    }
}
