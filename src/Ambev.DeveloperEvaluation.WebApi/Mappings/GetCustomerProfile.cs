using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetCustomerProfile : Profile
{
    public GetCustomerProfile()
    {
        CreateMap<GetCustomerResult, GetCustomerResponse>();
    }
}