using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Handler for processing DeleteCustomerCommand requests
/// </summary>
public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// Initializes a new instance of DeleteCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    public DeleteCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Handles the DeleteCustomerCommand request
    /// </summary>
    /// <param name="command">The DeleteCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the deletion operation</returns>
    public async Task<DeleteCustomerResult> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == Guid.Empty)
            throw new ValidationException("Customer ID cannot be empty");

        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.Id} not found");

        var success = await _customerRepository.DeleteAsync(command.Id, cancellationToken);

        return new DeleteCustomerResult
        {
            Success = success,
            Id = command.Id,
            Message = success ? "Customer deleted successfully" : "Failed to delete customer"
        };
    }
}