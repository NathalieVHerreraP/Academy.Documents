using Microsoft.AspNetCore.Builder;

namespace Academy.Documents.Presentation.Modules;

public class ModulesConfiguration
{
    public static void Configure (WebApplication app)
    {
        app.AddDocumentsModule();
    }
}
