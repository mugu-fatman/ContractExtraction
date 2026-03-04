namespace Frame.ContractExtraction.API.Models;

public sealed class ContractExtractionJsonRequest
{
    public required string BlobName { get; init; }

    // Values supplied by UI/user (master data / defaults)c, check if there is any better way to fetch these values rather than sending from UI
    public ContractDefaults Defaults { get; init; } = new();

    public Dictionary<string, object?> Overrides { get; init; } = new();
}

public sealed class ContractDefaults
{
    public string? ContractNumber { get; init; }          // generate if null
    public string? PropertyId { get; init; }              // UI-provided (Frame Property ID) 
    public string? ContractClass { get; init; }           // UI-provided
    public string? ContractType { get; init; }            // UI-provided
    public string? Currency { get; init; } = "EUR";       // default
    public string? PaymentFrequency { get; init; }        // e.g. Monthly , Quarterly, Annually - UI-provided
    public string? DueDate { get; init; }                 // ISO date or rule
    public bool? BoundToIndex { get; init; }              // true/false
    public decimal? LeaseLiability { get; init; }         // often not in PDF
    public decimal? RouAsset { get; init; }               // often not in PDF
}