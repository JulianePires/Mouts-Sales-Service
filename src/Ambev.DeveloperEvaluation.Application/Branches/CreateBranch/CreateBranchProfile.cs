using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Profile for mapping between Branch entity and CreateBranch command/result
/// </summary>
public class CreateBranchProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateBranch operation
    /// </summary>
    public CreateBranchProfile()
    {
        CreateMap<CreateBranchCommand, Branch>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Branch, CreateBranchResult>();
    }
}