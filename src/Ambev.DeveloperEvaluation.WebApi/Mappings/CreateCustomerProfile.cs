using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CreateCustomerProfile : Profile
{
    public CreateCustomerProfile()
    {
        CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
        CreateMap<CreateCustomerResult, CreateCustomerResponse>();
    }
}