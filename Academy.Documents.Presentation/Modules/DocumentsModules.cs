using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MediatR;
using System.Security.Cryptography;
using Academy.Documents.Application.Documents.Commands.SaveDocument;

namespace Academy.Documents.Presentation.Modules;

public static class DocumentsModules
{
    private const string BASE_URL = "/api/documents";

    public static void AddDocumentsModule (this IEndpointRouteBuilder app)
    {
        var documentsGroup = app.MapGroup(BASE_URL);
        documentsGroup.MapPost("SaveDocument/{userId}", SaveDocument).DisableAntiforgery(); ;
    }

    private static async Task<IResult> SaveDocument(
        [FromRoute] int userId,
        IFormFile file,
        CancellationToken cancellationToken,
        ISender sender)
    {
        using MemoryStream stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        byte[] fileBites = stream.ToArray();

        SaveDocumentCommandRequest request = new SaveDocumentCommandRequest
        {
            UserId = userId,
            FileName = file.FileName,
            ContentType = file.ContentType,
            Content = fileBites
        };

        SaveDocumentCommand command = new SaveDocumentCommand(request);
        var result = await sender.Send(command, cancellationToken);

        if(!result.IsSuccess)
        {
            switch (result.StatusCode)
            {
                case 400:
                    return Results.BadRequest(result);
                case 408:
                    return Results.Problem(result.Error?.ErrorCode,result.Error?.ErrorMessage);
                case 500:
                    return Results.Problem(result.Error?.ErrorCode, result.Error?.ErrorMessage);

            }
        }
        return Results.Ok("Document Saved Succesfully");
    }
}
