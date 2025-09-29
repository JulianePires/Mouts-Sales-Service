namespace Ambev.DeveloperEvaluation.Common.Security;

public interface IProduct
{
    /// <summary>
    /// Gets the unique identifier for the product.
    /// </summary>
    /// <returns>The Product ID as string.</returns>
    public string Id { get; }
    
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    /// <returns>The product name.</returns>
    public string Name { get; }
}