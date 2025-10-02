using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale, 
/// including customer ID, branch ID, and sale items. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CreateSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateSaleCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required business rules.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier for the customer making the purchase.
    /// Must be a valid existing customer ID.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the branch where the sale is made.
    /// Must be a valid existing branch ID.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the date when the sale is made.
    /// If not provided, current date will be used.
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the list of items being sold.
    /// Must contain at least one item and follow business rules for quantities and products.
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();

    /// <summary>
    /// Validates the command using the registered validator.
    /// </summary>
    /// <returns>A task that represents the asynchronous validation operation</returns>
    public async Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        var validator = new CreateSaleCommandValidator();
        var result = await validator.ValidateAsync(this);
        return result.Errors.Select(error => new ValidationErrorDetail
        {
            Error = error.PropertyName,
            Detail = error.ErrorMessage
        });
    }
}

/// <summary>
/// Represents a sale item request for creating a sale.
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the product being sold.
    /// Must be a valid existing product ID.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being sold.
    /// Must be between 1 and 20 according to business rules.
    /// </summary>
    public int Quantity { get; set; }
}