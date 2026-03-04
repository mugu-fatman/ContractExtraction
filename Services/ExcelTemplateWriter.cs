using ClosedXML.Excel;
using Frame.ContractExtraction.API.Models;

namespace Frame.ContractExtraction.API.Services;

public class ExcelTemplateWriter
{
    // IMPORTANT: Must match Frame import template headers EXACTLY
    private static readonly string[] Headers =
    {
        "Contract Number*",
        "Property ID*",
        "Contract Class*",
        "Contract Type*",
        "Contract Party*",
        "Due date*",
        "Payment Frequency*",
        "Currency*",
        "Quantity*",
        "Price*",
        "Start date of position*",
        "Lease liability*",
        "Rou asset*",
        "Bound to Index*",
        "Start date*",

        "Area m²",
        "Contact person",
        "Additional information",
        "End date of position",
        "Index type",
        "Index year",
        "Index month",
        "Base index point",
        "Minimum Increase %",
        "Maximum increase %",
        "First increase date",
        "How many increases per year",
        "Difference between increase month to comparison month",
        "Notes"
    };

    public byte[] GenerateExcel(LeaseContractDto dto)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Contracts");

        // Write header row
        for (int i = 0; i < Headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = Headers[i];
        }

        // Write data row
        object?[] values =
        {
            dto.ContractNumber,
            dto.PropertyId,
            dto.ContractClass,
            dto.ContractType,
            dto.ContractParty,
            dto.DueDate,
            dto.PaymentFrequency,
            dto.Currency,
            dto.Quantity,
            dto.Price,
            dto.StartDateOfPosition,
            dto.LeaseLiability,
            dto.RouAsset,
            dto.BoundToIndex,
            dto.StartDate,

            dto.AreaM2,
            dto.ContactPerson,
            dto.AdditionalInformation,
            dto.EndDateOfPosition,
            dto.IndexType,
            dto.IndexYear,
            dto.IndexMonth,
            dto.BaseIndexPoint,
            dto.MinimumIncreasePercent,
            dto.MaximumIncreasePercent,
            dto.FirstIncreaseDate,
            dto.IncreasesPerYear,
            dto.IncreaseMonthDiffToComparisonMonth,
            dto.Notes
        };

        for (int i = 0; i < values.Length; i++)
        {
            worksheet.Cell(2, i + 1).Value = values[i]?.ToString() ?? "";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}