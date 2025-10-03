using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetBranchProfile : Profile
{
    public GetBranchProfile()
    {
        CreateMap<GetBranchResult, GetBranchResponse>();
    }
}