namespace Frame.ContractExtraction.API.Models;

public sealed class ExtractTextResponse
{
    public string Blob { get; init; } = "";
    public List<ExtractedPage> Pages { get; init; } = new();
    public string FullText { get; init; } = "";
}