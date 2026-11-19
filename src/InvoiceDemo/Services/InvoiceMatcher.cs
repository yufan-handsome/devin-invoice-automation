namespace InvoiceDemo.Services;

public sealed class InvoiceMatcher
{
    public MatchResult Match(MatchRequest request)
    {
        var supplierMatches = Normalize(request.Supplier) == Normalize(request.PurchaseOrderSupplier);
        var difference = Math.Abs(request.InvoiceTotal - request.PurchaseOrderTotal);
        // Demo-only fixed tolerance; real reconciliation policies require explicit approval.
        var amountMatches = difference <= 0.01m;
        return new MatchResult(supplierMatches, amountMatches, difference, supplierMatches && amountMatches);
    }

    private static string Normalize(string name) =>
        new string(name.Where(char.IsLetterOrDigit).Select(char.ToUpperInvariant).ToArray());
}

public record MatchResult(bool SupplierMatches, bool AmountMatches, decimal Difference, bool RequiresNoReview);
