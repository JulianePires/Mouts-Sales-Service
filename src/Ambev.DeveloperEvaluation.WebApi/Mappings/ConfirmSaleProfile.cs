using Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for ConfirmSale feature mappings between API requests and commands
/// </summary>
public class ConfirmSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ConfirmSale feature
    /// </summary>
    public ConfirmSaleProfile()
    {
        CreateMap<Guid, ConfirmSaleCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

        CreateMap<ConfirmSaleResult, ConfirmSaleResponse>();
    }
}