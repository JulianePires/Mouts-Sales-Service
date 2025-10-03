namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Response model for DeleteProduct operation.
/// </summary>
public class DeleteProductResult
{
    /// <summary>
    /// Indicates whether the deletion was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The ID of the deleted product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Message describing the result of the operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}