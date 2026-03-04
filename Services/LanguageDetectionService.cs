using Azure;
using Azure.AI.TextAnalytics;

namespace Frame.ContractExtraction.API.Services;

public class LanguageDetectionService
{
    private readonly TextAnalyticsClient _client;

    public LanguageDetectionService(IConfiguration config)
    {
        var endpoint = new Uri(config["LanguageService:Endpoint"]!);
        var key = config["LanguageService:ApiKey"];
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key), "LanguageService:ApiKey configuration value is missing.");

        _client = new TextAnalyticsClient(endpoint, new AzureKeyCredential(key));
    }

    public async Task<string> DetectLanguageAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "en";

        var response = await _client.DetectLanguageAsync(text);

        return response.Value.Iso6391Name; // "en", "fi", etc.
    }
}