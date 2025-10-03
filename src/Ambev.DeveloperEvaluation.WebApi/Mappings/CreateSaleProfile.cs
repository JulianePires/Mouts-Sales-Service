using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for CreateSale feature mappings between API requests and commands
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale feature
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale.CreateSaleItemRequest,
                 Ambev.DeveloperEvaluation.Application.Sales.CreateSale.CreateSaleItemRequest>();

        CreateMap<CreateSaleResult, CreateSaleResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Draft"))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<CreateSaleItemResult, SaleItemResponse>()
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.DiscountPercent))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));
    }
}