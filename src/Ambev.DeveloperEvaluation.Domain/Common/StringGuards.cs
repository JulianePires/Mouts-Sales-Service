namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Helper methods for string validation and processing.
/// </summary>
public static class StringGuards
{
    /// <summary>
    /// Trims and validates that a string value is not null or whitespace for required fields.
    /// </summary>
    /// <param name="value">The string value to validate</param>
    /// <param name="paramName">The parameter name for error reporting</param>
    /// <param name="entityType">The entity type for error messages (e.g., "Customer", "Branch")</param>
    /// <returns>The trimmed string value</returns>
    /// <exception cref="ArgumentException">Thrown when value is null, empty, or whitespace</exception>
    public static string TrimRequired(this string? value, string paramName, string entityType = "")
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{entityType} {paramName} cannot be null or empty.", paramName);
        return value.Trim();
    }

    /// <summary>
    /// Cleans and validates a string value with optional validation logic.
    /// </summary>
    /// <param name="value">The string value to validate</param>
    /// <param name="paramName">The parameter name for error reporting</param>
    /// <param name="forbidEmpty">Whether to forbid empty strings</param>
    /// <param name="validator">Optional validation function</param>
    /// <param name="invalidMessage">Custom error message for validation failures</param>
    /// <returns>The trimmed and validated string value</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null and required</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    public static string CleanAndValidate(
        string? value,
        string paramName,
        bool forbidEmpty = false,
        Func<string, bool>? validator = null,
        string? invalidMessage = null)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        var trimmed = value.Trim();
        if (forbidEmpty && trimmed.Length == 0)
            throw new ArgumentException($"{paramName} cannot be empty or whitespace.", paramName);

        if (validator != null && !validator(trimmed))
            throw new ArgumentException(invalidMessage ?? $"Invalid {paramName}.", paramName);

        return trimmed;
    }
}