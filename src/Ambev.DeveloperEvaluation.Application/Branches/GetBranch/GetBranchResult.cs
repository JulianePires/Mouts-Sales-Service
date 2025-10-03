namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Response model for GetBranch operation.
/// </summary>
public class GetBranchResult
{
    /// <summary>
    /// The unique identifier of the branch.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the branch.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The physical address of the branch.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the branch.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the branch.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The name of the branch manager.
    /// </summary>
    public string Manager { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the branch is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The date when the branch was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date when the branch was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}