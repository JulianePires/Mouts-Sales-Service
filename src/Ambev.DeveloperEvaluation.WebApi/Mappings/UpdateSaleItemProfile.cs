using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for UpdateSaleItem feature mapping between API and Application layers
/// </summary>
public class UpdateSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSaleItem feature
    /// </summary>
    public UpdateSaleItemProfile()
    {
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();
        CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
    }
}