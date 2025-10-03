using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    private IServiceProvider? _services;
    private IMediator? _mediator;
    private IMapper? _mapper;

    protected IServiceProvider Services => _services ??= HttpContext.RequestServices;
    protected IMediator Mediator => _mediator ??= Services.GetRequiredService<IMediator>();
    protected IMapper Mapper => _mapper ??= Services.GetRequiredService<IMapper>();
    protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data) =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            Ok(new PaginatedResponse<T>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                Success = true
            });

    /// <summary>
    /// Generic pipeline for handling validate → map → send → map → respond workflow
    /// </summary>
    /// <typeparam name="TRequest">The request model type</typeparam>
    /// <typeparam name="TValidator">The validator type</typeparam>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <typeparam name="TCommandResult">The command result type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <param name="request">The request to validate and process</param>
    /// <param name="buildCommand">Function to build the command from the request</param>
    /// <param name="buildResponse">Function to build the response from the command result</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The action result</returns>
    protected async Task<IActionResult> HandleAsync<TRequest, TValidator, TCommand, TCommandResult, TResponse>(
        TRequest request,
        Func<TRequest, TCommand> buildCommand,
        Func<TCommandResult, IActionResult> buildResponse,
        CancellationToken cancellationToken = default)
        where TValidator : class, IValidator<TRequest>
        where TCommand : IRequest<TCommandResult>
    {
        var validator = Services.GetRequiredService<TValidator>();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = buildCommand(request);
        var result = await Mediator.Send(command, cancellationToken);

        return buildResponse(result);
    }
}
