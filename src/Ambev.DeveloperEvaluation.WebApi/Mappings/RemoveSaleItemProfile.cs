using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.RemoveSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for RemoveSaleItem feature mapping between API and Application layers
/// </summary>
public class RemoveSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for RemoveSaleItem feature
    /// </summary>
    public RemoveSaleItemProfile()
    {
        CreateMap<RemoveSaleItemResult, RemoveSaleItemResponse>();
    }
}