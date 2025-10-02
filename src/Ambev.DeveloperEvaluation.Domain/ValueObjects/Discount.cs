using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Represents a discount with percentage and amount values.
/// </summary>
public record Discount(decimal Percentage, decimal Amount)
{
    /// <summary>
    /// Gets a discount with no value applied.
    /// </summary>
    public static Discount None => new(0m, 0m);

    /// <summary>
    /// Creates a discount from a percentage and base value.
    /// </summary>
    /// <param name="percentage">The discount percentage (0-100)</param>
    /// <param name="baseValue">The base value to calculate discount from</param>
    /// <returns>A new discount instance</returns>
    public static Discount FromPercentage(decimal percentage, decimal baseValue)
    {
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Percentual deve estar entre 0 e 100");

        var amount = baseValue * (percentage / 100);
        return new Discount(percentage, amount);
    }

    /// <summary>
    /// Gets whether this discount has any value applied.
    /// </summary>
    public bool IsApplied => Percentage > 0 && Amount > 0;
}