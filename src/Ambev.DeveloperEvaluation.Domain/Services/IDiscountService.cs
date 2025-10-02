using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service for calculating quantity-based discounts on sales items.
/// </summary>
public interface IDiscountService
{
    /// <summary>
    /// Calculates the discount based on quantity and unit price.
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="unitPrice">The unit price of the item</param>
    /// <returns>The calculated discount</returns>
    Discount CalculateDiscount(int quantity, decimal unitPrice);

    /// <summary>
    /// Validates quantity limits according to business rules.
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    void ValidateQuantityLimits(int quantity);

    /// <summary>
    /// Determines if the quantity is eligible for discount.
    /// </summary>
    /// <param name="quantity">The quantity to check</param>
    /// <returns>True if eligible for discount, false otherwise</returns>
    bool IsEligibleForDiscount(int quantity);
}