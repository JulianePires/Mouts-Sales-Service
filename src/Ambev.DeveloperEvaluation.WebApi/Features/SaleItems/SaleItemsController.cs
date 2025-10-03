using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.AddSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.UpdateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.RemoveSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems;

/// <summary>
/// Controller for managing sale items
/// </summary>
[ApiController]
[Route("api/Sales/{saleId}/items")]
public class SaleItemsController : BaseController
{
    /// <summary>
    /// Initializes a new instance of SaleItemsController
    /// </summary>
    public SaleItemsController()
    {
    }

    /// <summary>
    /// Retrieves a specific sale item
    /// </summary>
    /// <param name="saleId">The ID of the sale containing the item</param>
    /// <param name="itemId">The ID of the item to retrieve</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale item details</returns>
    [HttpGet("{itemId}", Name = "GetSaleItem")]
    [ProducesResponseType(typeof(AddSaleItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleItem(
        [FromRoute] Guid saleId,
        [FromRoute] Guid itemId,
        CancellationToken cancellationToken = default)
    {
        // For simplicity, get the full sale and return the specific item
        // In a real application, you might want a dedicated GetSaleItem use case
        var getSaleCommand = new GetSaleCommand(saleId);
        var saleResult = await Mediator.Send(getSaleCommand, cancellationToken);

        var item = saleResult.Items.FirstOrDefault(x => x.Id == itemId);
        if (item == null)
            return NotFound("Sale item not found");

        var response = new AddSaleItemResponse
        {
            Id = item.Id,
            SaleId = saleId,
            ProductId = item.Product.Id,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.TotalPrice,
            Discount = item.DiscountPercent
        };

        return Ok(response);
    }

    /// <summary>
    /// Adds a new item to a sale
    /// </summary>
    /// <param name="saleId">The ID of the sale to add the item to</param>
    /// <param name="request">The sale item data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AddSaleItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSaleItem(
        [FromRoute] Guid saleId,
        [FromBody] AddSaleItemRequest request,
        CancellationToken cancellationToken = default)
    {
        return await HandleAsync<AddSaleItemRequest, AddSaleItemRequestValidator, AddSaleItemCommand, AddSaleItemResult, AddSaleItemResponse>(
            request,
            r =>
            {
                var command = Mapper.Map<AddSaleItemCommand>(r);
                command.SaleId = saleId;
                return command;
            },
            result =>
            {
                var response = Mapper.Map<AddSaleItemResponse>(result);
                return CreatedAtRoute("GetSaleItem", new { saleId = saleId, itemId = response.Id }, response);
            },
            cancellationToken);
    }

    /// <summary>
    /// Updates the quantity of a sale item
    /// </summary>
    /// <param name="saleId">The ID of the sale containing the item</param>
    /// <param name="itemId">The ID of the item to update</param>
    /// <param name="request">The update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale item details</returns>
    [HttpPut("{itemId}")]
    [ProducesResponseType(typeof(UpdateSaleItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleItem(
        [FromRoute] Guid saleId,
        [FromRoute] Guid itemId,
        [FromBody] UpdateSaleItemRequest request,
        CancellationToken cancellationToken = default)
    {
        return await HandleAsync<UpdateSaleItemRequest, UpdateSaleItemRequestValidator, UpdateSaleItemCommand, UpdateSaleItemResult, UpdateSaleItemResponse>(
            request,
            r =>
            {
                var command = Mapper.Map<UpdateSaleItemCommand>(r);
                command.SaleId = saleId;
                command.ItemId = itemId;
                return command;
            },
            result =>
            {
                var response = Mapper.Map<UpdateSaleItemResponse>(result);
                return Ok(response);
            },
            cancellationToken);
    }

    /// <summary>
    /// Removes an item from a sale
    /// </summary>
    /// <param name="saleId">The ID of the sale containing the item</param>
    /// <param name="itemId">The ID of the item to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The removal operation result</returns>
    [HttpDelete("{itemId}")]
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

        var response = await Mediator.Send(command, cancellationToken);
        var result = Mapper.Map<RemoveSaleItemResponse>(response);

        return Ok(result);
    }
}