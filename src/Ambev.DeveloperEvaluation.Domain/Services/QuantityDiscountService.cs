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
        // Reuse existing validation methods
        ValidateQuantityLimits(quantity);
        ValidateUnitPrice(unitPrice);

        var subtotal = quantity * unitPrice;

        // Reuse eligibility check
        if (!IsEligibleForDiscount(quantity))
            return Discount.None;

        // Compute tiered discount
        var discountPercentage = quantity < BusinessConstants.MediumDiscountQuantity
            ? BusinessConstants.LowTierDiscountPercentage
            : BusinessConstants.HighTierDiscountPercentage;

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

        if (quantity > BusinessConstants.MaximumQuantityPerProduct)
            throw new DomainException($"Não é possível vender mais de {BusinessConstants.MaximumQuantityPerProduct} itens idênticos");
    }

    /// <summary>
    /// Validates unit price according to business rules.
    /// </summary>
    /// <param name="unitPrice">The unit price to validate</param>
    /// <exception cref="DomainException">Thrown when unit price is invalid</exception>
    private void ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice <= 0)
            throw new DomainException("Preço unitário deve ser maior que zero");

        if (unitPrice > BusinessConstants.MaximumUnitPrice)
            throw new DomainException("Preço unitário excede o limite máximo permitido");
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