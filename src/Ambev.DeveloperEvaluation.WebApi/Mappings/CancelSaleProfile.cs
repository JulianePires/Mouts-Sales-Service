using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for CancelSale feature mappings between API requests and commands
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelSale feature
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<Guid, CancelSaleCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}