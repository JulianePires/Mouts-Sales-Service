using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a sale transaction in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// Implements automatic discount calculation based on quantity purchased.
/// </summary>
public class SaleItem : BaseEntity, ISaleItem
{
    /// <summary>
    /// Gets the unique identifier for the sale associated with this item.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets the product associated with the sale item.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Gets the quantity of the product sold in this item.
    /// Must be between 1 and 20 (business rule for maximum quantity per product).
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets the discount percentage applied to this sale item.
    /// Automatically calculated based on quantity: 4-9 items = 10%, 10-20 items = 20%.
    /// </summary>
    public decimal DiscountPercent { get; set; }

    /// <summary>
    /// Gets the price of the product at the time of sale.
    /// Captures the product price to maintain historical accuracy.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets the total price for this sale item after discount application.
    /// Calculated as (Quantity * UnitPrice) - discount.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gets the cancellation status of the sale item.
    /// When cancelled, the item is excluded from sale totals.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets the date and time when the sale item was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the sale item.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the unique identifier of the sale item.
    /// </summary>
    /// <returns>The sale item ID.</returns>
    Guid ISaleItem.Id => Id;

    /// <summary>
    /// Gets the unique identifier for the sale associated with this item.
    /// </summary>
    /// <returns>The Sale ID.</returns>
    Guid ISaleItem.SaleId => SaleId;

    /// <summary>
    /// Gets the product associated with the sale item.
    /// </summary>
    /// <returns>The product information.</returns>
    IProduct ISaleItem.Product => Product;

    /// <summary>
    /// Gets the quantity of the product sold in this item.
    /// </summary>
    /// <returns>The quantity sold.</returns>
    int ISaleItem.Quantity => Quantity;

    /// <summary>
    /// Gets the discount percentage applied to this sale item.
    /// </summary>
    /// <returns>The discount percentage (0-100).</returns>
    decimal ISaleItem.DiscountPercent => DiscountPercent;

    /// <summary>
    /// Gets the price of the product at the time of sale.
    /// </summary>
    /// <returns>The unit price of the product.</returns>
    decimal ISaleItem.UnitPrice => UnitPrice;

    /// <summary>
    /// Gets the total price for this sale item after discount.
    /// </summary>
    /// <returns>The total price for the sale item.</returns>
    decimal ISaleItem.TotalPrice => TotalPrice;

    /// <summary>
    /// Gets the cancellation status of the sale item.
    /// </summary>
    /// <returns>True if the item is cancelled; otherwise, false.</returns>
    bool ISaleItem.IsCancelled => IsCancelled;

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem()
    {
        CreatedAt = DateTime.UtcNow.AddMilliseconds(-BusinessConstants.DateValidationBufferMilliseconds); // Slightly in the past to avoid validation timing issues
    }

    /// <summary>
    /// Creates a new sale item with the specified information.
    /// Automatically calculates discount and total price based on business rules.
    /// </summary>
    /// <param name="saleId">The sale ID this item belongs to.</param>
    /// <param name="product">The product being sold.</param>
    /// <param name="quantity">The quantity being sold.</param>
    /// <param name="unitPrice">The unit price at time of sale.</param>
    /// <returns>A new SaleItem instance.</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when business rules are violated.</exception>
    public static SaleItem Create(Guid saleId, Product product, int quantity, decimal? unitPrice = null)
    {
        // Parameter validation (domain responsibility)
        if (saleId == Guid.Empty)
            throw new ArgumentException("Sale ID cannot be empty.", nameof(saleId));

        if (product == null)
            throw new ArgumentException("Product cannot be null.", nameof(product));

        // Business rules validation
        if (quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 units of the same product in a single sale.");

        var finalUnitPrice = unitPrice ?? product.Price;

        var saleItem = new SaleItem
        {
            SaleId = saleId,
            Product = product,
            Quantity = quantity,
            UnitPrice = finalUnitPrice
        };

        saleItem.CalculateDiscountAndTotal();
        return saleItem;
    }

    /// <summary>
    /// Cancels the sale item.
    /// Sets the item as cancelled and excludes it from calculations.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;
        TotalPrice = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reactivates a cancelled sale item.
    /// Recalculates the total price and discount.
    /// </summary>
    public void Reactivate()
    {
        if (!IsCancelled)
            return;

        IsCancelled = false;
        CalculateDiscountAndTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the quantity and recalculates discount and total.
    /// </summary>
    /// <param name="newQuantity">The new quantity for the item.</param>
    /// <exception cref="ArgumentException">Thrown when quantity is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when business rules are violated or item is cancelled.</exception>
    public void UpdateQuantity(int newQuantity)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot update quantity of a cancelled item.");

        // Format validation (newQuantity > 0) handled by FluentValidation in handlers
        // Business rule: quantity limit handled at Sale level for proper product aggregation

        Quantity = newQuantity;
        CalculateDiscountAndTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the discount percentage and total price based on business rules.
    /// Business Rules:
    /// - 4 to 9 items: 10% discount
    /// - 10 to 20 items: 20% discount
    /// - Less than 4 items: No discount
    /// </summary>
    private void CalculateDiscountAndTotal()
    {
        if (IsCancelled)
        {
            TotalPrice = 0;
            return;
        }

        // Calculate discount based on quantity
        DiscountPercent = Quantity switch
        {
            >= 10 and <= 20 => 20m,
            >= 4 and < 10 => 10m,
            _ => 0m
        };

        var subtotal = Quantity * UnitPrice;
        var discountAmount = subtotal * (DiscountPercent / 100m);
        TotalPrice = Math.Round(subtotal - discountAmount, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Gets the discount amount in currency.
    /// </summary>
    /// <returns>The discount amount applied to this item.</returns>
    public decimal GetDiscountAmount()
    {
        if (IsCancelled)
            return 0;

        var subtotal = Quantity * UnitPrice;
        return Math.Round(subtotal * (DiscountPercent / 100m), 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Gets the subtotal before discount application.
    /// </summary>
    /// <returns>The subtotal before discount.</returns>
    public decimal GetSubtotal()
    {
        return IsCancelled ? 0 : Quantity * UnitPrice;
    }

    /// <summary>
    /// Performs validation of the sale item entity using the SaleItemValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
