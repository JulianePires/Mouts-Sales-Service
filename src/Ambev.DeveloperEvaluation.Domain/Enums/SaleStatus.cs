namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents the possible states of a sale in the system
/// </summary>
public enum SaleStatus
{
    /// <summary>
    /// Sale is in draft state - being constructed
    /// </summary>
    Draft,

    /// <summary>
    /// Sale has been confirmed and processed
    /// </summary>
    Confirmed,

    /// <summary>
    /// Sale has been cancelled and is no longer valid
    /// </summary>
    Cancelled
}