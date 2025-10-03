using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;
using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches;

[ApiController]
[Route("api/branches")]
public class BranchesController : BaseController
{
    [HttpPost]
    public Task<IActionResult> CreateBranch(
        [FromBody] CreateBranchRequest request,
        CancellationToken cancellationToken) =>
        HandleAsync<CreateBranchRequest, CreateBranchRequestValidator, CreateBranchCommand, CreateBranchResult, CreateBranchResponse>(
            request,
            r => Mapper.Map<CreateBranchCommand>(r),
            r => Created("GetBranch", new { id = r.Id }, Mapper.Map<CreateBranchResponse>(r)),
            cancellationToken);

    [HttpGet("{id}")]
    public Task<IActionResult> GetBranch(
        [FromRoute] Guid id,
        CancellationToken cancellationToken) =>
        HandleAsync<GetBranchRequest, GetBranchRequestValidator, GetBranchCommand, GetBranchResult, GetBranchResponse>(
            new GetBranchRequest { Id = id },
            r => new GetBranchCommand(r.Id),
            r => Ok(Mapper.Map<GetBranchResponse>(r)),
            cancellationToken);
}