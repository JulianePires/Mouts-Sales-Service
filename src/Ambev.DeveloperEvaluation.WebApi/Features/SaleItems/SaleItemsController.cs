using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.AddSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.UpdateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.RemoveSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems;

/// <summary>
/// Controller for managing sale items
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SaleItemsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SaleItemsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SaleItemsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Adds a new item to a sale
    /// </summary>
    /// <param name="saleId">The ID of the sale to add the item to</param>
    /// <param name="request">The sale item data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item details</returns>
    [HttpPost("/api/sales/{saleId}/items")]
    [ProducesResponseType(typeof(AddSaleItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSaleItem(
        [FromRoute] Guid saleId,
        [FromBody] AddSaleItemRequest request,
        CancellationToken cancellationToken = default)
    {
        var validator = new AddSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<AddSaleItemCommand>(request);
        command.SaleId = saleId;

        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<AddSaleItemResponse>(response);

        return Created($"/api/sales/{saleId}/items/{result.Id}", result);
    }

    /// <summary>
    /// Updates the quantity of a sale item
    /// </summary>
    /// <param name="saleId">The ID of the sale containing the item</param>
    /// <param name="itemId">The ID of the item to update</param>
    /// <param name="request">The update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale item details</returns>
    [HttpPut("/api/sales/{saleId}/items/{itemId}")]
    [ProducesResponseType(typeof(UpdateSaleItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleItem(
        [FromRoute] Guid saleId,
        [FromRoute] Guid itemId,
        [FromBody] UpdateSaleItemRequest request,
        CancellationToken cancellationToken = default)
    {
        var validator = new UpdateSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleItemCommand>(request);
        command.SaleId = saleId;
        command.ItemId = itemId;

        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<UpdateSaleItemResponse>(response);

        return Ok(result);
    }

    /// <summary>
    /// Removes an item from a sale
    /// </summary>
    /// <param name="saleId">The ID of the sale containing the item</param>
    /// <param name="itemId">The ID of the item to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The removal operation result</returns>
    [HttpDelete("/api/sales/{saleId}/items/{itemId}")]
    [ProducesResponseType(typeof(RemoveSaleItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveSaleItem(
        [FromRoute] Guid saleId,
        [FromRoute] Guid itemId,
        CancellationToken cancellationToken = default)
    {
        var command = new RemoveSaleItemCommand
        {
            SaleId = saleId,
            ItemId = itemId
        };

        var response = await _mediator.Send(command, cancellationToken);
        var result = _mapper.Map<RemoveSaleItemResponse>(response);

        return Ok(result);
    }
}