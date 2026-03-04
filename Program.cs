
using Frame.ContractExtraction.API.Services;


var builder = WebApplication.CreateBuilder(args);

// Services (must be before Build)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<BlobService>();
builder.Services.AddSingleton<DocumentIntelligenceService>();
builder.Services.AddSingleton<IContractExtractor, OpenAiContractExtractor>();
builder.Services.AddSingleton<ContractMerger>();
builder.Services.AddSingleton<ContractValidator>();
builder.Services.AddSingleton<ExcelTemplateWriter>();
builder.Services.AddSingleton<LanguageDetectionService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();