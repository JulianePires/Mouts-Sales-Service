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

    /// <summary>
    /// Calculates the discount based on quantity and unit price.
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="unitPrice">The unit price of the item</param>
    /// <returns>The calculated discount</returns>
    public Discount CalculateDiscount(int quantity, decimal unitPrice)
    {
        // Merged all validations in one place
        if (quantity <= 0 || quantity > BusinessConstants.MaximumQuantityPerProduct)
            throw new DomainException(quantity <= 0
                ? "Quantidade deve ser maior que zero"
                : $"Não é possível vender mais de {BusinessConstants.MaximumQuantityPerProduct} itens idênticos");

        if (unitPrice <= 0 || unitPrice > BusinessConstants.MaximumUnitPrice)
            throw new DomainException(unitPrice <= 0
                ? "Preço unitário deve ser maior que zero"
                : "Preço unitário excede o limite máximo permitido");

        var subtotal = quantity * unitPrice;

        // Single switch for all discount logic (includes "no-discount" tier)
        return quantity switch
        {
            < BusinessConstants.MinimumDiscountQuantity => Discount.None,
            < BusinessConstants.MediumDiscountQuantity => Discount.FromPercentage(BusinessConstants.LowTierDiscountPercentage, subtotal),
            <= BusinessConstants.MaximumQuantityPerProduct => Discount.FromPercentage(BusinessConstants.HighTierDiscountPercentage, subtotal),
            _ => throw new DomainException($"Quantidade {quantity} está fora dos limites permitidos")
        };
    }

    /// <summary>
    /// Validates quantity limits according to business rules.
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    public void ValidateQuantityLimits(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero");

        if (quantity > BusinessConstants.MaximumQuantityPerProduct)
            throw new DomainException($"Não é possível vender mais de {BusinessConstants.MaximumQuantityPerProduct} itens idênticos");
    }

    /// <summary>
    /// Determines if the quantity is eligible for discount.
    /// </summary>
    /// <param name="quantity">The quantity to check</param>
    /// <returns>True if eligible for discount, false otherwise</returns>
    public bool IsEligibleForDiscount(int quantity)
    {
        return quantity >= BusinessConstants.MinimumDiscountQuantity && quantity <= BusinessConstants.MaximumQuantityPerProduct;
    }
}