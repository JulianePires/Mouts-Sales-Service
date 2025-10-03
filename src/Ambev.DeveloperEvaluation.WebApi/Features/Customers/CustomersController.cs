using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers;

[ApiController]
[Route("api/Customers")]
public class CustomersController : BaseController
{
    [HttpPost]
    public Task<IActionResult> CreateCustomer(
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken) =>
        HandleAsync<CreateCustomerRequest, CreateCustomerRequestValidator, CreateCustomerCommand, CreateCustomerResult, CreateCustomerResponse>(
            request,
            r => Mapper.Map<CreateCustomerCommand>(r),
            r => Created("GetCustomer", new { id = r.Id }, Mapper.Map<CreateCustomerResponse>(r)),
            cancellationToken);

    [HttpGet("{id}")]
    public Task<IActionResult> GetCustomer(
        [FromRoute] Guid id,
        CancellationToken cancellationToken) =>
        HandleAsync<GetCustomerRequest, GetCustomerRequestValidator, GetCustomerCommand, GetCustomerResult, GetCustomerResponse>(
            new GetCustomerRequest { Id = id },
            r => new GetCustomerCommand(r.Id),
            r => Ok(Mapper.Map<GetCustomerResponse>(r)),
            cancellationToken);

    [HttpPut("{id}")]
    public Task<IActionResult> UpdateCustomer(
        [FromRoute] Guid id,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken) =>
        HandleAsync<UpdateCustomerRequest, UpdateCustomerRequestValidator, UpdateCustomerCommand, UpdateCustomerResult, UpdateCustomerResponse>(
            request,
            r => { var cmd = Mapper.Map<UpdateCustomerCommand>(r); cmd.Id = id; return cmd; },
            r => Ok(Mapper.Map<UpdateCustomerResponse>(r)),
            cancellationToken);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(Mapper.Map<DeleteCustomerResponse>(result));
    }
}