using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system with customer, branch, and items information.
/// This entity follows domain-driven design principles and includes comprehensive business rules validation.
/// Manages the complete sale lifecycle including item management, discount calculation, and cancellation.
/// </summary>
public class Sale : BaseEntity, ISale
{
    /// <summary>
    /// Gets the sale number for identification and tracking purposes.
    /// Automatically generated with a unique format when not provided.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets the date when the sale was made.
    /// Used for reporting and business analytics.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets the unique identifier for the customer associated with this sale.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets the unique identifier for the branch associated with this sale.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets the customer associated with this sale.
    /// Contains customer information for the transaction.
    /// </summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>
    /// Gets the branch where the sale was made.
    /// Identifies the location of the transaction.
    /// </summary>
    public Branch Branch { get; set; } = null!;

    /// <summary>
    /// Gets the collection of items included in the sale.
    /// Each item represents a product with quantity, price, and discount information.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Gets the total amount for the sale after all discounts.
    /// Automatically calculated from active (non-cancelled) items.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets the current status of the sale.
    /// Used to track sale lifecycle from draft to confirmed or cancelled.
    /// </summary>
    public SaleStatus Status { get; set; } = SaleStatus.Draft;

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the unique identifier of the sale.
    /// </summary>
    /// <returns>The sale's ID as a string.</returns>
    string ISale.Id => Id.ToString();

    /// <summary>
    /// Gets the sale number.
    /// </summary>
    /// <returns>The sale number.</returns>
    string ISale.SaleNumber => SaleNumber;

    /// <summary>
    /// Gets the date when the sale was made.
    /// </summary>
    /// <returns>The sale date.</returns>
    DateTime ISale.SaleDate => SaleDate;

    /// <summary>
    /// Gets the customer data.
    /// </summary>
    /// <returns>The customer information.</returns>
    ICustomer ISale.Customer => Customer;

    /// <summary>
    /// Gets the data of the branch where the sale was made.
    /// </summary>
    /// <returns>The branch information.</returns>
    IBranch ISale.Branch => Branch;

    /// <summary>
    /// Gets the list of items included in the sale.
    /// </summary>
    /// <returns>A readonly collection of sale items.</returns>
    IReadOnlyCollection<ISaleItem> ISale.Items => Items.Cast<ISaleItem>().ToList().AsReadOnly();

    /// <summary>
    /// Gets the total amount for the sale.
    /// </summary>
    /// <returns>The total sale amount.</returns>
    decimal ISale.TotalAmount => TotalAmount;

    /// <summary>
    /// Gets the cancellation status of the sale.
    /// </summary>
    /// <returns>True if the sale is cancelled; otherwise, false.</returns>
    bool ISale.IsCancelled => Status == SaleStatus.Cancelled;

    /// <summary>
    /// Gets the date when the sale was created.
    /// </summary>
    /// <returns>The sale creation date.</returns>
    DateTime ISale.CreatedAt => CreatedAt;

    /// <summary>
    /// Gets the date of the last update to the sale.
    /// </summary>
    /// <returns>The last update date, if any.</returns>
    DateTime? ISale.UpdatedAt => UpdatedAt;

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        Id = Guid.NewGuid(); // Ensure Sale has a valid ID when created
        CreatedAt = DateTime.UtcNow.AddMilliseconds(-BusinessConstants.DateValidationBufferMilliseconds); // Slightly in the past to avoid validation timing issues
        SaleDate = DateTime.UtcNow;
        Items = new List<SaleItem>();
    }

    /// <summary>
    /// Creates a new sale with the specified information.
    /// Automatically generates a sale number if not provided.
    /// </summary>
    /// <param name="customer">The customer making the purchase.</param>
    /// <param name="branch">The branch where the sale is made.</param>
    /// <param name="saleNumber">Optional sale number (auto-generated if null).</param>
    /// <param name="saleDate">Optional sale date (current date if null).</param>
    /// <returns>A new Sale instance.</returns>
    /// <exception cref="ArgumentException">Thrown when required parameters are null.</exception>
    public static Sale Create(Customer customer, Branch branch, string saleNumber, DateTime? saleDate = null)
    {
        if (customer == null)
            throw new ArgumentException("Customer cannot be null.", nameof(customer));

        if (branch == null)
            throw new ArgumentException("Branch cannot be null.", nameof(branch));

        if (!customer.IsActive)
            throw new InvalidOperationException("Cannot create sale for inactive customer.");

        if (!branch.IsActive)
            throw new InvalidOperationException("Cannot create sale for inactive branch.");

        return new Sale
        {
            Customer = customer,
            Branch = branch,
            SaleNumber = saleNumber,
            SaleDate = saleDate ?? DateTime.UtcNow
        };
    }

    /// <summary>
    /// Adds an item to the sale with automatic discount calculation.
    /// Validates business rules including maximum quantity per product.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <param name="quantity">The quantity to add.</param>
    /// <param name="unitPrice">Optional unit price (uses product price if null).</param>
    /// <returns>The created SaleItem.</returns>
    /// <exception cref="InvalidOperationException">Thrown when sale is cancelled or business rules are violated.</exception>
    public SaleItem AddItem(Product product, int quantity, decimal? unitPrice = null)
    {
        if (Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("Cannot add items to a cancelled sale.");

        if (product == null)
            throw new ArgumentException("Product cannot be null.", nameof(product));

        if (!product.IsAvailableForSale())
            throw new InvalidOperationException("Product is not available for sale.");

        // Business Rule: Check if adding this quantity would exceed the 20-unit limit for this product
        var existingQuantity = Items
            .Where(i => !i.IsCancelled && i.Product.Id == product.Id)
            .Sum(i => i.Quantity);

        if (existingQuantity + quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 units of the same product in a single sale.");

        // Business Rule: Check stock availability (consider total quantity including existing in sale)
        var totalRequiredStock = existingQuantity + quantity;
        if (product.StockQuantity < totalRequiredStock)
            throw new InvalidOperationException($"Product '{product.Name}' is not available in the requested quantity. Available: {product.StockQuantity}, Required for sale: {totalRequiredStock}.");

        // Business Rule: Check if adding this product would exceed the 20 different products limit
        var existingProductIds = Items
            .Where(i => !i.IsCancelled)
            .Select(i => i.Product.Id)
            .Distinct()
            .Count();

        var isNewProduct = !Items.Any(i => !i.IsCancelled && i.Product.Id == product.Id);
        if (isNewProduct && existingProductIds >= 20)
            throw new InvalidOperationException("Cannot add more than 20 different products to a single sale.");

        var saleItem = SaleItem.Create(Id, product, quantity, unitPrice);
        Items.Add(saleItem);
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
        return saleItem;
    }

    /// <summary>
    /// Removes an item from the sale by cancelling it.
    /// Recalculates the total amount after removal.
    /// </summary>
    /// <param name="itemId">The ID of the item to remove.</param>
    /// <exception cref="InvalidOperationException">Thrown when sale is cancelled or item not found.</exception>
    public void RemoveItem(Guid itemId)
    {
        if (Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("Cannot remove items from a cancelled sale.");

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new ArgumentException("Item not found in sale.", nameof(itemId));

        item.Cancel();
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the quantity of an existing item in the sale.
    /// Validates business rules and recalculates totals.
    /// </summary>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <param name="newQuantity">The new quantity for the item.</param>
    /// <param name="product">The product to validate stock against.</param>
    /// <exception cref="InvalidOperationException">Thrown when sale is cancelled, item not found, or business rules violated.</exception>
    public void UpdateItemQuantity(Guid itemId, int newQuantity, Product product)
    {
        if (Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("Cannot update items in a cancelled sale.");

        var item = Items.FirstOrDefault(i => i.Id == itemId && !i.IsCancelled);
        if (item == null)
            throw new ArgumentException("Item not found in sale.", nameof(itemId));

        // Business Rule: Check stock availability considering total quantity of this product in the sale
        var otherItemsQuantity = Items
            .Where(i => !i.IsCancelled && i.Product.Id == product.Id && i.Id != itemId)
            .Sum(i => i.Quantity);

        var totalRequiredStock = otherItemsQuantity + newQuantity;
        if (product.StockQuantity < totalRequiredStock)
        {
            throw new InvalidOperationException($"Product '{product.Name}' does not have sufficient stock for this update. Available: {product.StockQuantity}, Required for sale: {totalRequiredStock}.");
        }

        // Business Rule: Check if updating this quantity would exceed the 20-unit limit for this product
        if (otherItemsQuantity + newQuantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 units of the same product in a single sale.");

        item.UpdateQuantity(newQuantity);
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the entire sale.
    /// Sets the sale as cancelled and zeros the total amount.
    /// </summary>
    public void Cancel()
    {
        if (Status == SaleStatus.Cancelled)
            return;

        Status = SaleStatus.Cancelled;
        TotalAmount = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reactivates a cancelled sale.
    /// Recalculates the total amount from active items.
    /// </summary>
    public void Reactivate()
    {
        if (Status != SaleStatus.Cancelled)
            throw new InvalidOperationException("Sale is not cancelled.");

        Status = SaleStatus.Draft;
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Confirms the sale, marking it as final and processed.
    /// Validates business rules before confirmation.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when sale cannot be confirmed due to validation errors.</exception>
    public void Confirm()
    {
        if (Status == SaleStatus.Confirmed)
            return;

        if (Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("Cannot confirm a cancelled sale.");

        if (!HasItems())
            throw new InvalidOperationException("Cannot confirm sale without items.");

        Status = SaleStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the total discount amount applied to the sale.
    /// </summary>
    /// <returns>The total discount amount across all items.</returns>
    public decimal GetTotalDiscount()
    {
        if (Status == SaleStatus.Cancelled)
            return 0;

        return Items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.GetDiscountAmount());
    }

    /// <summary>
    /// Gets the subtotal before discount application.
    /// </summary>
    /// <returns>The subtotal before discounts.</returns>
    public decimal GetSubtotal()
    {
        if (Status == SaleStatus.Cancelled)
            return 0;

        return Items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.GetSubtotal());
    }

    /// <summary>
    /// Gets the number of active (non-cancelled) items in the sale.
    /// </summary>
    /// <returns>The count of active items.</returns>
    public int GetActiveItemCount()
    {
        return Items.Count(i => !i.IsCancelled);
    }

    /// <summary>
    /// Checks if the sale has any items.
    /// </summary>
    /// <returns>True if the sale has active items; otherwise, false.</returns>
    public bool HasItems()
    {
        return GetActiveItemCount() > 0;
    }

    /// <summary>
    /// Recalculates the total amount from all active items.
    /// </summary>
    private void RecalculateTotal()
    {
        if (Status == SaleStatus.Cancelled)
        {
            TotalAmount = 0;
            return;
        }

        TotalAmount = Items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.TotalPrice);
    }

    /// <summary>
    /// Performs comprehensive validation of the sale entity using the SaleValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
