using System.Text.Json;
using Frame.ContractExtraction.API.Models;

namespace Frame.ContractExtraction.API.Services;

public sealed class ContractMerger
{
    public LeaseContractDto ApplyDefaultsAndOverrides(
        LeaseContractDto extracted,
        ContractDefaults defaults,
        Dictionary<string, object?> overrides)
    {
        extracted.ContractNumber ??= defaults.ContractNumber
            ?? $"LEASE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}".Substring(0, 18);

        extracted.PropertyId ??= defaults.PropertyId;
        extracted.ContractClass ??= defaults.ContractClass;
        extracted.ContractType ??= defaults.ContractType;

        extracted.Currency ??= defaults.Currency;
        extracted.PaymentFrequency ??= defaults.PaymentFrequency;
        extracted.DueDate ??= defaults.DueDate;
        extracted.BoundToIndex ??= defaults.BoundToIndex;

        extracted.LeaseLiability ??= defaults.LeaseLiability;
        extracted.RouAsset ??= defaults.RouAsset;

        // Optional overrides (UI edits)
        foreach (var kv in overrides)
        {
            var prop = typeof(LeaseContractDto).GetProperty(kv.Key);
            if (prop is null || !prop.CanWrite) continue;

            if (kv.Value is null)
            {
                prop.SetValue(extracted, null);
                continue;
            }

            var json = JsonSerializer.Serialize(kv.Value);
            var typed = JsonSerializer.Deserialize(json, prop.PropertyType);
            prop.SetValue(extracted, typed);
        }

        return extracted;
    }
}