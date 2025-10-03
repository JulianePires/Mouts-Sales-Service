using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Handler for processing GetCustomerCommand requests
/// </summary>
public class GetCustomerHandler : IRequestHandler<GetCustomerCommand, GetCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCustomerHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCustomerCommand request
    /// </summary>
    /// <param name="command">The GetCustomer command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer details if found</returns>
    public async Task<GetCustomerResult> Handle(GetCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Customer with ID {command.Id} not found");

        return _mapper.Map<GetCustomerResult>(customer);
    }
}