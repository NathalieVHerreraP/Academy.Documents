using Academy.Documents.Domain.Shared;
using MediatR;

namespace Academy.Documents.Application.Documents.Commands.SaveDocument;

public sealed record SaveDocumentCommand(SaveDocumentCommandRequest request):IRequest<Result>;
