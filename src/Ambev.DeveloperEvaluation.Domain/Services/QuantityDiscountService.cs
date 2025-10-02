using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Implementation of discount service that applies discounts based on quantity tiers.
/// Business Rules:
/// - 4-9 items: 10% discount
/// - 10-20 items: 20% discount
/// - Maximum 20 items per product
/// </summary>
public class QuantityDiscountService : IDiscountService
{
    private const int MinimumDiscountQuantity = 4;
    private const int MediumDiscountQuantity = 10;
    private const int MaximumQuantity = 20;
    private const decimal LowTierDiscountPercentage = 10m;
    private const decimal HighTierDiscountPercentage = 20m;

    /// <summary>
    /// Calculates the discount based on quantity and unit price.
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="unitPrice">The unit price of the item</param>
    /// <returns>The calculated discount</returns>
    public Discount CalculateDiscount(int quantity, decimal unitPrice)
    {
        ValidateInputs(quantity, unitPrice);

        var discountPercentage = GetDiscountPercentage(quantity);

        if (discountPercentage == 0)
            return Discount.None;

        var subtotal = quantity * unitPrice;
        return Discount.FromPercentage(discountPercentage, subtotal);
    }

    /// <summary>
    /// Validates quantity limits according to business rules.
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    public void ValidateQuantityLimits(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero");

        if (quantity > MaximumQuantity)
            throw new DomainException($"Não é possível vender mais de {MaximumQuantity} itens idênticos");
    }

    /// <summary>
    /// Determines if the quantity is eligible for discount.
    /// </summary>
    /// <param name="quantity">The quantity to check</param>
    /// <returns>True if eligible for discount, false otherwise</returns>
    public bool IsEligibleForDiscount(int quantity)
    {
        return quantity >= MinimumDiscountQuantity && quantity <= MaximumQuantity;
    }

    /// <summary>
    /// Gets the discount percentage based on quantity tier.
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <returns>The discount percentage to apply</returns>
    private decimal GetDiscountPercentage(int quantity)
    {
        return quantity switch
        {
            < MinimumDiscountQuantity => 0m,
            >= MinimumDiscountQuantity and < MediumDiscountQuantity => LowTierDiscountPercentage,
            >= MediumDiscountQuantity and <= MaximumQuantity => HighTierDiscountPercentage,
            _ => throw new DomainException($"Quantidade {quantity} está fora dos limites permitidos")
        };
    }

    /// <summary>
    /// Validates input parameters for discount calculation.
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    /// <param name="unitPrice">The unit price to validate</param>
    private void ValidateInputs(int quantity, decimal unitPrice)
    {
        ValidateQuantityLimits(quantity);

        if (unitPrice <= 0)
            throw new DomainException("Preço unitário deve ser maior que zero");

        if (unitPrice > 1_000_000m) // Limit to prevent overflow
            throw new DomainException("Preço unitário excede o limite máximo permitido");
    }
}