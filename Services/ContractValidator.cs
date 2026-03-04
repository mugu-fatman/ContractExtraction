using Frame.ContractExtraction.API.Models;

namespace Frame.ContractExtraction.API.Services;

public sealed class ContractValidator
{
    public List<string> GetMissingRequiredFields(LeaseContractDto dto)
    {
        var missing = new List<string>();

        void Req(string name, object? value)
        {
            var ok = value switch
            {
                null => false,
                string s => !string.IsNullOrWhiteSpace(s),
                _ => true
            };
            if (!ok) missing.Add(name);
        }

        Req(nameof(dto.ContractNumber), dto.ContractNumber);
        Req(nameof(dto.PropertyId), dto.PropertyId);
        Req(nameof(dto.ContractClass), dto.ContractClass);
        Req(nameof(dto.ContractType), dto.ContractType);
        Req(nameof(dto.ContractParty), dto.ContractParty);
        Req(nameof(dto.DueDate), dto.DueDate);
        Req(nameof(dto.PaymentFrequency), dto.PaymentFrequency);
        Req(nameof(dto.Currency), dto.Currency);
        Req(nameof(dto.Quantity), dto.Quantity);
        Req(nameof(dto.Price), dto.Price);
        Req(nameof(dto.StartDateOfPosition), dto.StartDateOfPosition);
        Req(nameof(dto.LeaseLiability), dto.LeaseLiability);
        Req(nameof(dto.RouAsset), dto.RouAsset);
        Req(nameof(dto.BoundToIndex), dto.BoundToIndex);
        Req(nameof(dto.StartDate), dto.StartDate);

        return missing;
    }
}