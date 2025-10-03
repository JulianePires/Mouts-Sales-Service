using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class DeleteCustomerProfile : Profile
{
    public DeleteCustomerProfile()
    {
        CreateMap<DeleteCustomerResult, DeleteCustomerResponse>();
    }
}