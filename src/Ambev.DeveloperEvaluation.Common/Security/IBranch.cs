namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Defines the contract for representing a branch in the system.
/// </summary>
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
    
    /// <summary>
    /// Gets the address of the branch.
    /// </summary>
    /// <returns>The branch address.</returns>
    public string Address { get; }
    
    /// <summary>
    /// Gets the phone number of the branch.
    /// </summary>
    /// <returns>The branch phone number.</returns>
    public string Phone { get; }
    
    /// <summary>
    /// Gets the email of the branch.
    /// </summary>
    /// <returns>The branch email.</returns>
    public string Email { get; }
    
    /// <summary>
    /// Gets the branch manager name.
    /// </summary>
    /// <returns>The branch manager name.</returns>
    public string Manager { get; }
    
    /// <summary>
    /// Gets the active status of the branch.
    /// </summary>
    /// <returns>True if the branch is active; otherwise, false.</returns>
    public bool IsActive { get; }
}