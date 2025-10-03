using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Handler for processing UpdateCustomerCommand requests
/// </summary>
public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateCustomerCommand request
    /// </summary>
    /// <param name="command">The UpdateCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated customer details</returns>
    public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCustomerCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.Id} not found");

        if (!string.IsNullOrEmpty(command.Email) && command.Email != customer.Email)
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingCustomer != null)
                throw new InvalidOperationException($"Customer with email {command.Email} already exists");
        }

        customer.UpdateInfo(command.Name, command.Email, command.Phone, command.Address, command.BirthDate);

        if (command.IsActive.HasValue)
        {
            if (command.IsActive.Value)
                customer.Activate();
            else
                customer.Deactivate();
        }

        var updatedCustomer = await _customerRepository.UpdateAsync(customer, cancellationToken);
        return _mapper.Map<UpdateCustomerResult>(updatedCustomer);
    }
}