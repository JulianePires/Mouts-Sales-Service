namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Contains business rule constants that can be configured externally in the future.
/// Centralizes magic numbers and makes them easily discoverable and maintainable.
/// </summary>
public static class BusinessConstants
{
    /// <summary>
    /// Maximum number of units of the same product allowed in a single sale.
    /// </summary>
    public const int MaximumQuantityPerProduct = 20;

    /// <summary>
    /// Maximum unit price allowed for any product.
    /// </summary>
    public const decimal MaximumUnitPrice = 1_000_000m;

    /// <summary>
    /// Minimum quantity required to be eligible for any discount.
    /// </summary>
    public const int MinimumDiscountQuantity = 4;

    /// <summary>
    /// Quantity threshold for medium tier discount.
    /// </summary>
    public const int MediumDiscountQuantity = 10;

    /// <summary>
    /// Discount percentage for low tier (4-9 items).
    /// </summary>
    public const decimal LowTierDiscountPercentage = 10m;

    /// <summary>
    /// Discount percentage for high tier (10-20 items).
    /// </summary>
    public const decimal HighTierDiscountPercentage = 20m;

    /// <summary>
    /// Buffer for date validation to handle timing differences between entity creation and validation.
    /// </summary>
    public const int DateValidationBufferMilliseconds = 2000;
}