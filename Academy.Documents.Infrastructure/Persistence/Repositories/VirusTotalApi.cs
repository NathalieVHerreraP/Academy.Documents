using Academy.Documents.Domain.Entities.DocumentsEntity.Repositories;
using VirusTotalNet;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;

namespace Academy.Documents.Infrastructure.Persistence.Repositories;

public class VirusTotalApi : IVirusTotalApi
{
    private const string APIKEY = "cb23f7ffc1cf35a65f916350c81100578720ed5d25504f35c45394673e42b0ad";
    public async Task<bool> ScanDocument(byte[] content, string fileName)
    {
        using VirusTotal virusTotal = new(APIKEY);
        //Use Https
        virusTotal.UseTLS = true;

        ScanResult report = await virusTotal.ScanFileAsync(content, fileName);

        FileReport fileReport = await virusTotal.GetFileReportAsync(report.Resource);

        while (fileReport.ResponseCode == FileReportResponseCode.Queued)
        {
            await Task.Delay(15000);
            fileReport = await virusTotal.GetFileReportAsync(report.Resource);
        }

        if(fileReport.ResponseCode == FileReportResponseCode.Present)
        {
            return fileReport.Positives == 0;
        }
        else
        {
            throw new Exception($"An error occurred while getting file report: { fileReport.VerboseMsg }");
        }


    }
}
