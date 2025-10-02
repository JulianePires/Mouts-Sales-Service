using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

/// <summary>
/// AutoMapper profile for Sale-related mappings between domain entities and application DTOs
/// </summary>
public class SaleMappingProfile : Profile
{
    /// <summary>
    /// Initializes the mapping configuration for Sale entities
    /// </summary>
    public SaleMappingProfile()
    {
        CreateSaleMappings();
        GetSaleMappings();
    }

    /// <summary>
    /// Creates mappings for CreateSale use case
    /// </summary>
    private void CreateSaleMappings()
    {
        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<SaleItem, CreateSaleItemResult>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }

    /// <summary>
    /// Creates mappings for GetSale use case
    /// </summary>
    private void GetSaleMappings()
    {
        CreateMap<Sale, GetSaleResult>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<Customer, GetSaleCustomerInfo>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<Branch, GetSaleBranchInfo>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

        CreateMap<SaleItem, GetSaleItemResult>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

        CreateMap<Product, GetSaleProductInfo>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}