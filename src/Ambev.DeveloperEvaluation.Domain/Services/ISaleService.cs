using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleService
    {
        Task<Sale> EnsureActiveSaleAsync(Guid saleId, CancellationToken cancellationToken = default);
        SaleItem EnsureActiveItem(Sale sale, Guid itemId);
    }
}