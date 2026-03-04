namespace Frame.ContractExtraction.API.Models;

public sealed class ExtractedPage
{
    public int PageNumber { get; init; }
    public string Text { get; init; } = "";
}