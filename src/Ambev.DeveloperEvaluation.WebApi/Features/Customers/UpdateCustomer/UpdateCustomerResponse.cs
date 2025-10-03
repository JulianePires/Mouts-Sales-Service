namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;

public class UpdateCustomerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}