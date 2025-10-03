namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Response model for DeleteCustomer operation.
/// </summary>
public class DeleteCustomerResult
{
    /// <summary>
    /// Indicates whether the deletion was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The ID of the deleted customer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Message describing the result of the operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}