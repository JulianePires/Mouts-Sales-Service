using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for AddSaleItem feature mapping between API and Application layers
/// </summary>
public class AddSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for AddSaleItem feature
    /// </summary>
    public AddSaleItemProfile()
    {
        CreateMap<AddSaleItemRequest, AddSaleItemCommand>();
        CreateMap<AddSaleItemResult, AddSaleItemResponse>();
    }
}