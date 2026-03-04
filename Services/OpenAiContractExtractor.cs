using System.Text.Json;
using Azure;
using Azure.AI.OpenAI;
using Frame.ContractExtraction.API.Models;

namespace Frame.ContractExtraction.API.Services;

public interface IContractExtractor
{
    Task<LeaseContractDto> ExtractAsync(string fullText, CancellationToken ct);
}

public sealed class OpenAiContractExtractor : IContractExtractor
{
    private readonly OpenAIClient _client;
    private readonly string _deployment;

    public OpenAiContractExtractor(IConfiguration cfg)
    {
        _client = new OpenAIClient(
            new Uri(cfg["AzureOpenAI:Endpoint"]!),
            new AzureKeyCredential(cfg["AzureOpenAI:ApiKey"]!)
        );
        _deployment = cfg["AzureOpenAI:Deployment"]!;
    }

    public async Task<LeaseContractDto> ExtractAsync(string fullText, CancellationToken ct)
    {
        // JSON “shape hint” (keeps output stable)
        var shapeHint = JsonSerializer.Serialize(new LeaseContractDto(),
            new JsonSerializerOptions { WriteIndented = true });

var system = """
You are a contract data extraction engine.

OUTPUT FORMAT (STRICT):
- Return ONLY valid JSON.
- Do not wrap JSON in markdown.
- Do not include explanations, comments, or trailing text.

TYPE RULES (VERY IMPORTANT):
- BoundToIndex MUST be a JSON boolean: true or false (NOT "true"/"false", NOT "Yes"/"No").
- Numeric fields MUST be JSON numbers (NOT strings). Example: 90500 not "90500" and not "90,500".
- Percent fields are numbers without % sign. Example: 2.5 not "2.5%".
- Dates MUST be strings in ISO format YYYY-MM-DD.
- If a value is not explicitly present in the document, return null.

EXTRACTION RULES:
- Do NOT invent values.
- Do NOT calculate values unless the document explicitly states the calculated total.
- Prefer values explicitly stated as totals (e.g., “total annual rent”) over derived values.
- If multiple candidates exist, pick the clearest one and add a short note to ExtractionNotes.

EXAMPLES (FOLLOW EXACTLY):
Example boolean:
{ "BoundToIndex": true }

Example numbers:
{ "Quantity": 480, "Price": 90500, "BaseIndexPoint": 123.45 }

Example date:
{ "StartDate": "2020-06-01" }
""";

var user = $"""
Return JSON that matches this exact shape (same property names). Keep properties you cannot find as null.

JSON shape:
{shapeHint}

Document text:
{fullText}
""";

        var options = new ChatCompletionsOptions
        {
            
        };
        options.Messages.Add(new ChatRequestSystemMessage(system));
        options.Messages.Add(new ChatRequestUserMessage(user));
        options.DeploymentName = _deployment;

        var resp = await _client.GetChatCompletionsAsync(options, ct);
        var json = resp.Value.Choices[0].Message.Content;

        var dto = JsonSerializer.Deserialize<LeaseContractDto>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (dto is null)
            throw new InvalidOperationException("OpenAI returned invalid JSON for LeaseContractDto.");

        return dto;
    }
}