using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/products")]
public class ProductsController : BaseController
{
    [HttpPost]
    public Task<IActionResult> CreateProduct(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken) =>
        HandleAsync<CreateProductRequest, CreateProductRequestValidator, CreateProductCommand, CreateProductResult, CreateProductResponse>(
            request,
            r => Mapper.Map<CreateProductCommand>(r),
            r => Created("GetProduct", new { id = r.Id }, Mapper.Map<CreateProductResponse>(r)),
            cancellationToken);

    [HttpGet("{id}")]
    public Task<IActionResult> GetProduct(
        [FromRoute] Guid id,
        CancellationToken cancellationToken) =>
        HandleAsync<GetProductRequest, GetProductRequestValidator, GetProductCommand, GetProductResult, GetProductResponse>(
            new GetProductRequest { Id = id },
            r => new GetProductCommand(r.Id),
            r => Ok(Mapper.Map<GetProductResponse>(r)),
            cancellationToken);

    [HttpPut("{id}")]
    public Task<IActionResult> UpdateProduct(
        [FromRoute] Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken) =>
        HandleAsync<UpdateProductRequest, UpdateProductRequestValidator, UpdateProductCommand, UpdateProductResult, UpdateProductResponse>(
            request,
            r => { var cmd = Mapper.Map<UpdateProductCommand>(r); cmd.Id = id; return cmd; },
            r => Ok(Mapper.Map<UpdateProductResponse>(r)),
            cancellationToken);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(Mapper.Map<DeleteProductResponse>(result));
    }
}