using Frame.ContractExtraction.API.Models;
using Frame.ContractExtraction.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frame.ContractExtraction.API.Controllers;

[ApiController]
[Route("api/contracts")]
public sealed class ContractExtractionController : ControllerBase
{
    private readonly BlobService _blob;
    private readonly DocumentIntelligenceService _di;
    private readonly IContractExtractor _extractor;
    private readonly ContractMerger _merger;
    private readonly ContractValidator _validator;

    public ContractExtractionController(
        BlobService blob,
        DocumentIntelligenceService di,
        IContractExtractor extractor,
        ContractMerger merger,
        ContractValidator validator)
    {
        _blob = blob;
        _di = di;
        _extractor = extractor;
        _merger = merger;
        _validator = validator;
    }

    [HttpPost("extract-json")]
    public async Task<ActionResult<ContractExtractionJsonResponse>> ExtractJson(
        [FromBody] ContractExtractionJsonRequest request,
        CancellationToken ct)
    {
        var pdfBytes = await _blob.DownloadAsync(request.BlobName);
        var pages = await _di.ExtractPagesAsync(pdfBytes, ct);
        var fullText = DocumentIntelligenceService.JoinPages(pages);

        var extracted = await _extractor.ExtractAsync(fullText, ct);
        extracted = _merger.ApplyDefaultsAndOverrides(extracted, request.Defaults, request.Overrides);

        var missing = _validator.GetMissingRequiredFields(extracted);

        return Ok(new ContractExtractionJsonResponse
        {
            Blob = request.BlobName,
            Contract = extracted,
            MissingRequiredFields = missing
        });
    }

    [HttpPost("extract-excel")]
    public async Task<IActionResult> ExtractExcel(
        [FromBody] ContractExtractionJsonRequest request,
        [FromServices] ExcelTemplateWriter excelWriter,
        CancellationToken ct)
    {
        var pdfBytes = await _blob.DownloadAsync(request.BlobName);

        var pages = await _di.ExtractPagesAsync(pdfBytes, ct);
        var fullText = DocumentIntelligenceService.JoinPages(pages);
        var extracted = await _extractor.ExtractAsync(fullText, ct);
        extracted = _merger.ApplyDefaultsAndOverrides(extracted, request.Defaults, request.Overrides);

        var missing = _validator.GetMissingRequiredFields(extracted);

        if (missing.Any())
        {
            return BadRequest(new
            {
                message = "Missing required fields",
                missing
            });
        }

        var excelBytes = excelWriter.GenerateExcel(extracted);

        return File(
            excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "LeaseImport.xlsx");
    }
}