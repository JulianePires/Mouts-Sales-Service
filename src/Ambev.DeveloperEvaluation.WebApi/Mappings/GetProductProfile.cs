using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<GetProductResult, GetProductResponse>();
    }
}