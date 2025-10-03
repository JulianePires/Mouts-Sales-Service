using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Command for retrieving a branch by their ID.
/// </summary>
public class GetBranchCommand : IRequest<GetBranchResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to retrieve.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of GetBranchCommand.
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    public GetBranchCommand(Guid id)
    {
        Id = id;
    }
}