using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Tests;

[ApiController]
[Route("api/[controller]")]
public class EventTestController : ControllerBase
{
    private readonly IDomainEventService _domainEventService;

    public EventTestController(IDomainEventService domainEventService)
    {
        _domainEventService = domainEventService;
    }

    [HttpPost("test-event")]
    public async Task<IActionResult> TestEvent()
    {
        try
        {
            // Create a test event
            var testEvent = new SaleCreated(new Domain.Entities.Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = "TEST-001",
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow
            });

            // Process the event
            await _domainEventService.ProcessEventsAsync(new[] { testEvent });

            return Ok(new { message = "Event processed successfully", eventId = testEvent.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}