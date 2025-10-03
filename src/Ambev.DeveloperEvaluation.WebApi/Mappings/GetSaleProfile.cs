using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for GetSale feature mappings between API requests and commands
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale feature
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Guid, GetSaleCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

        CreateMap<GetSaleResult, GetSaleResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsCancelled ? "Cancelled" : "Active"))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<GetSaleCustomerInfo, CustomerInfo>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<GetSaleBranchInfo, BranchInfo>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Address));

        CreateMap<GetSaleItemResult, SaleItemInfo>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.DiscountPercent));
    }
}