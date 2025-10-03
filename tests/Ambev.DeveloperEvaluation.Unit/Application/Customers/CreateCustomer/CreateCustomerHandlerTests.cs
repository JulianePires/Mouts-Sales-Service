using FluentValidation;
using NSubstitute;
using Xunit;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application.Customers.CreateCustomer;

/// <summary>
/// Contains unit tests for the <see cref="CreateCustomerHandler"/> class.
/// </summary>
public class CreateCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly CreateCustomerHandler _handler;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateCustomerHandlerTests"/>.
    /// </summary>
    public CreateCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateCustomerHandler(_customerRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid customer creation request succeeds.
    /// </summary>
    [Fact(DisplayName = "Handle should create customer successfully when request is valid")]
    public async Task Handle_ValidRequest_CreatesCustomerSuccessfully()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = "(11) 99999-9999",
            Address = "123 Main Street",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var customer = new Customer { Id = Guid.NewGuid() };
        var result = new CreateCustomerResult { Id = customer.Id };

        _customerRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        _mapper.Map<Customer>(command).Returns(customer);
        _customerRepository.CreateAsync(customer, Arg.Any<CancellationToken>())
            .Returns(customer);
        _mapper.Map<CreateCustomerResult>(customer).Returns(result);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(result.Id, response.Id);
        await _customerRepository.Received(1).CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that validation exception is thrown when command is invalid.
    /// </summary>
    [Fact(DisplayName = "Handle should throw ValidationException when command is invalid")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            Name = "", // Invalid: empty name
            Email = "invalid-email",
            Phone = "123",
            Address = ""
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        await _customerRepository.DidNotReceive().CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that InvalidOperationException is thrown when customer email already exists.
    /// </summary>
    [Fact(DisplayName = "Handle should throw InvalidOperationException when email already exists")]
    public async Task Handle_ExistingEmail_ThrowsInvalidOperationException()
    {
        // Arrange  
        var command = new CreateCustomerCommand
        {
            Name = "John Doe",
            Email = "existing@example.com",
            Phone = "(11) 99999-9999",
            Address = "123 Main Street",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var existingCustomer = new Customer { Email = command.Email };
        _customerRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(existingCustomer);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("already exists", exception.Message);
        await _customerRepository.DidNotReceive().CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that phone number validation works correctly.
    /// </summary>
    [Theory(DisplayName = "Handle should validate phone number format")]
    [InlineData("(11) 99999-9999", true)]
    [InlineData("(21) 8888-8888", true)]
    [InlineData("11 99999-9999", false)]
    [InlineData("(11) 999999999", false)]
    [InlineData("123", false)]
    public async Task Handle_PhoneValidation_WorksCorrectly(string phone, bool shouldSucceed)
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Phone = phone,
            Address = "123 Main Street",
            BirthDate = new DateTime(1990, 1, 1)
        };

        _customerRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        if (shouldSucceed)
        {
            var customer = new Customer { Id = Guid.NewGuid() };
            var result = new CreateCustomerResult { Id = customer.Id };

            _mapper.Map<Customer>(command).Returns(customer);
            _customerRepository.CreateAsync(customer, Arg.Any<CancellationToken>())
                .Returns(customer);
            _mapper.Map<CreateCustomerResult>(customer).Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
        }
        else
        {
            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }
    }
}