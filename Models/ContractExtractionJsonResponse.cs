namespace Frame.ContractExtraction.API.Models;

public sealed class ContractExtractionJsonResponse
{
    public string Blob { get; init; } = "";
    public LeaseContractDto Contract { get; init; } = new();
    public List<string> MissingRequiredFields { get; init; } = new();
}