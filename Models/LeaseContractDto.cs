namespace Frame.ContractExtraction.API.Models;

public sealed class LeaseContractDto
{
    // Mandatory (as per template)
    public string? ContractNumber { get; set; }
    public string? PropertyId { get; set; }
    public string? ContractClass { get; set; }
    public string? ContractType { get; set; }
    public string? ContractParty { get; set; }
    public string? DueDate { get; set; }
    public string? PaymentFrequency { get; set; }
    public string? Currency { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? Price { get; set; }
    public string? StartDateOfPosition { get; set; }
    public decimal? LeaseLiability { get; set; }
    public decimal? RouAsset { get; set; }
    public bool? BoundToIndex { get; set; }
    public string? StartDate { get; set; }

    // Optional
    public decimal? AreaM2 { get; set; }
    public string? ContactPerson { get; set; }
    public string? AdditionalInformation { get; set; }
    public string? EndDateOfPosition { get; set; }
    public string? IndexType { get; set; }
    public int? IndexYear { get; set; }
    public int? IndexMonth { get; set; }
    public decimal? BaseIndexPoint { get; set; }
    public decimal? MinimumIncreasePercent { get; set; }
    public decimal? MaximumIncreasePercent { get; set; }
    public string? FirstIncreaseDate { get; set; }
    public int? IncreasesPerYear { get; set; }
    public int? IncreaseMonthDiffToComparisonMonth { get; set; }
    public string? Notes { get; set; }

    // Debug
    public List<string> ExtractionNotes { get; set; } = new();
}