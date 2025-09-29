namespace Ambev.DeveloperEvaluation.Common.Security;

public interface IBranch
{
    /// <summary>
    /// Gets the unique identifier for the branch.
    /// </summary>
    /// <returns>The Branch ID as string.</returns>
    public string Id { get; }
    
    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    /// <returns>The branch name.</returns>
    public string Name { get; }
}