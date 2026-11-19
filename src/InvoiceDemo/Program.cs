using InvoiceDemo.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<InvoiceMatcher>();
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapPost("/invoices/match", (MatchRequest request, InvoiceMatcher matcher) =>
{
    if (string.IsNullOrWhiteSpace(request.Supplier) || string.IsNullOrWhiteSpace(request.PurchaseOrderSupplier))
        return Results.BadRequest(new { error = "Supplier names are required." });
    if (request.InvoiceTotal < 0 || request.PurchaseOrderTotal < 0)
        return Results.BadRequest(new { error = "Totals must be non-negative." });
    return Results.Ok(matcher.Match(request));
});

app.Run();

public record MatchRequest(string Supplier, string PurchaseOrderSupplier, decimal InvoiceTotal, decimal PurchaseOrderTotal);
