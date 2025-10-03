namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.RemoveSaleItem;

/// <summary>
/// Response model for remove sale item operation
/// </summary>
public class RemoveSaleItemResponse
{
    /// <summary>
    /// Indicates whether the removal was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message describing the result of the operation
    /// </summary>
    public string Message { get; set; } = string.Empty;
}