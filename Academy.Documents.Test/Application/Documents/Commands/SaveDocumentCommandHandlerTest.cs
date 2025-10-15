using Academy.Documents.Application.Documents.Commands.SaveDocument;
using Academy.Documents.Domain.Entities.DocumentsEntity;
using Academy.Documents.Domain.Entities.DocumentsEntity.Repositories;
using Moq;

namespace Academy.Documents.Test.Application.Documents.Commands;

public class SaveDocumentCommandHandlerTest
{
    private readonly Mock<IDocumentsRepository> _mockDocs = new();
    private readonly Mock<IVirusTotalApi> _mockVirusTotal = new();
    private readonly CancellationToken _cancellationToken = new();

    [Fact]
    public async Task Returns_Document_Saved_Ok()
    {
        // Arrange
        SaveDocumentCommandRequest request = new SaveDocumentCommandRequest
        {
            UserId = 1,
            FileName = "test.pdf",
            ContentType = "application/pdf",
            Content = new byte[] { 0x25, 0x50, 0x44, 0x46 } 
        };
        _mockDocs.Setup(x => x.SaveDocument(It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockDocs.Setup(x => x.RegisterdocumentLog(It.IsAny<DocumentLog>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask); 
        _mockVirusTotal.Setup(x => x.ScanDocument(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(true);

        //Act
        var command = new SaveDocumentCommand(request);
        var handler = new SaveDocumentCommandHandler(_mockDocs.Object, _mockVirusTotal.Object);
        var result = await handler.Handle(command,_cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
    [Fact]
    public async Task Returns_TaskConcelledException()
    {
        // Arrange
        SaveDocumentCommandRequest request = new SaveDocumentCommandRequest
        {
            UserId = 1,
            FileName = "test.pdf",
            ContentType = "application/pdf",
            Content = new byte[] { 0x25, 0x50, 0x44, 0x46 }
        };
        CancellationToken cancelationTokenTrue = new(true); 
        _mockVirusTotal.Setup(x => x.ScanDocument(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(true);

        //Act
        var command = new SaveDocumentCommand(request);
        var handler = new SaveDocumentCommandHandler(_mockDocs.Object, _mockVirusTotal.Object);
        var result = await handler.Handle(command, cancelationTokenTrue);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(408, result.StatusCode);
        Assert.Equal("Task Cancelled", result.Error?.ErrorMessage);

    }

    [Fact]
    public async Task Returns_Document_Infected()
    {
        // Arrange
        SaveDocumentCommandRequest request = new SaveDocumentCommandRequest
        {
            UserId = 1,
            FileName = "test.pdf",
            ContentType = "application/pdf",
            Content = new byte[] { 0x25, 0x50, 0x44, 0x46 } 
        };
        _mockDocs.Setup(x => x.RegisterdocumentLog(It.IsAny<DocumentLog>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockVirusTotal.Setup(x => x.ScanDocument(It.IsAny<byte[]>(), It.IsAny<string>())).ReturnsAsync(false);

        //Act
        var command = new SaveDocumentCommand(request);
        var handler = new SaveDocumentCommandHandler(_mockDocs.Object, _mockVirusTotal.Object);
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400,result.StatusCode);
        Assert.Equal("The document can not be saved because it is infected with virus", result.Error?.ErrorMessage);
    }
    [Fact]
    public async Task Returns_Invalid_UserId()
    {
        // Arrange
        SaveDocumentCommandRequest request = new SaveDocumentCommandRequest
        {
            UserId = 0,
            FileName = "test.pdf",
            ContentType = "application/pdf",
            Content = new byte[] { 0x25, 0x50, 0x44, 0x46 } 
        };

        //Act
        var command = new SaveDocumentCommand(request);
        var handler = new SaveDocumentCommandHandler(_mockDocs.Object, _mockVirusTotal.Object);
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid Data", result.Error?.ErrorMessage);
    }
    [Fact]
    public async Task Returns_Invalid_Document_ContentType()
    {
        // Arrange
        SaveDocumentCommandRequest request = new SaveDocumentCommandRequest
        {
            UserId = 1,
            FileName = "test.pdf",
            ContentType = "application/vnd.microsoft.portable-executable",
            Content = new byte[] { 0x25, 0x50, 0x44, 0x46 } 
        };

        //Act
        var command = new SaveDocumentCommand(request);
        var handler = new SaveDocumentCommandHandler(_mockDocs.Object, _mockVirusTotal.Object);
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid Data", result.Error?.ErrorMessage);
    }

    [Fact]
    public async Task Returns_Invalid_Document_Empty_Content()
    {
        // Arrange
        SaveDocumentCommandRequest request = new SaveDocumentCommandRequest
        {
            UserId = 1,
            FileName = "test.pdf",
            ContentType = "application/pdf",
            Content = new byte[] {}
        };

        //Act
        var command = new SaveDocumentCommand(request);
        var handler = new SaveDocumentCommandHandler(_mockDocs.Object, _mockVirusTotal.Object);
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid Data", result.Error?.ErrorMessage);
    }
}
