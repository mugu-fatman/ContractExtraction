using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Frame.ContractExtraction.API.Models;

namespace Frame.ContractExtraction.API.Services;

public class DocumentIntelligenceService
{
    private readonly DocumentAnalysisClient _client;

    public DocumentIntelligenceService(IConfiguration config)
    {
        var endpoint = new Uri(config["DocumentIntelligence:Endpoint"]!);
        var apiKey = config["DocumentIntelligence:ApiKey"]!;
        _client = new DocumentAnalysisClient(endpoint, new AzureKeyCredential(apiKey));
    }

    public async Task<List<ExtractedPage>> ExtractPagesAsync(byte[] pdfBytes, CancellationToken ct)
    {
        using var stream = new MemoryStream(pdfBytes);

        var operation = await _client.AnalyzeDocumentAsync(
            WaitUntil.Completed,
            "prebuilt-read",
            stream,
            cancellationToken: ct);

        var result = operation.Value;

        var pages = new List<ExtractedPage>();

        foreach (var page in result.Pages)
        {
            var sb = new System.Text.StringBuilder();

            foreach (var line in page.Lines)
                sb.AppendLine(line.Content);

            pages.Add(new ExtractedPage
            {
                PageNumber = page.PageNumber,
                Text = sb.ToString().Trim()
            });
        }

        return pages;
    }

    public static string JoinPages(List<ExtractedPage> pages)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var p in pages.OrderBy(p => p.PageNumber))
        {
            sb.AppendLine($"--- Page {p.PageNumber} ---");
            sb.AppendLine(p.Text);
            sb.AppendLine();
        }
        return sb.ToString().Trim();
    }
}