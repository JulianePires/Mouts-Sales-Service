using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

// Temporarily commented out due to ambiguous call issue\n// using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.ORM.Repositories;

/// <summary>
/// Contains unit tests for the <see cref="BranchRepository"/> class.
/// Tests cover CRUD operations, filtering, searching, and specific business logic methods.
/// </summary>
public class BranchRepositoryTests : IDisposable
{
    private readonly DefaultContext _context;
    private readonly BranchRepository _repository;

    public BranchRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DefaultContext(options);
        _repository = new BranchRepository(_context);
    }

    /// <summary>
    /// Tests that CreateAsync successfully creates a branch when valid data is provided.
    /// </summary>
    [Fact(DisplayName = "Given valid branch When creating Then should return created branch")]
    public async Task CreateAsync_WithValidBranch_ShouldReturnCreatedBranch()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        // Act
        var result = await _repository.CreateAsync(branch);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(branch.Name);
        result.Address.Should().Be(branch.Address);
        result.Manager.Should().Be(branch.Manager);
    }

    /// <summary>
    /// Tests that CreateAsync throws an exception when null branch is provided.
    /// </summary>
    [Fact(DisplayName = "Given null branch When creating Then should throw ArgumentNullException")]
    public async Task CreateAsync_WithNullBranch_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await FluentActions.Invoking(() => _repository.CreateAsync(null!))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that GetByIdAsync returns branch when valid ID is provided.
    /// </summary>
    [Fact(DisplayName = "Given existing branch ID When getting by ID Then should return branch")]
    public async Task GetByIdAsync_WithValidId_ShouldReturnBranch()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Name = "Test Branch";
        branch.Address = "456 Test St";
        branch.Phone = "(11) 88888-8888";
        branch.Manager = "Test Manager";
        branch.IsActive = true;

        var createdBranch = await _repository.CreateAsync(branch);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(createdBranch.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(createdBranch.Id);
        result.Name.Should().Be(branch.Name);
    }

    /// <summary>
    /// Tests that GetByIdAsync returns null when branch ID does not exist.
    /// </summary>
    [Fact(DisplayName = "Given non-existing branch ID When getting by ID Then should return null")]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that GetByAddressAsync returns branches with specified address.
    /// </summary>
    [Fact(DisplayName = "Given branches with different addresses When getting by address Then should return filtered branches")]
    public async Task GetByAddressAsync_WithValidAddress_ShouldReturnFilteredBranches()
    {
        // Arrange
        var address = "Main Street";
        var branch1 = BranchTestData.GenerateValidBranch();
        branch1.Address = "123 Main Street";
        var branch2 = BranchTestData.GenerateValidBranch();
        branch2.Address = "456 Main Street";
        var branch3 = BranchTestData.GenerateValidBranch();
        branch3.Address = "789 Oak Avenue";

        await _repository.CreateAsync(branch1);
        await _repository.CreateAsync(branch2);
        await _repository.CreateAsync(branch3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetByAddressAsync(address);

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(b => b.Address.Contains(address));
    }

    /// <summary>
    /// Tests that GetByManagerAsync returns branches managed by specified manager.
    /// </summary>
    [Fact(DisplayName = "Given branches with different managers When getting by manager Then should return manager's branches")]
    public async Task GetByManagerAsync_WithValidManager_ShouldReturnManagerBranches()
    {
        // Arrange
        var manager = "John Smith";
        var branch1 = BranchTestData.GenerateValidBranch();
        branch1.Manager = manager;
        var branch2 = BranchTestData.GenerateValidBranch();
        branch2.Manager = manager;
        var branch3 = BranchTestData.GenerateValidBranch();
        branch3.Manager = "Jane Doe";

        await _repository.CreateAsync(branch1);
        await _repository.CreateAsync(branch2);
        await _repository.CreateAsync(branch3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetByManagerAsync(manager);

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(b => b.Manager == manager);
    }

    /// <summary>
    /// Tests that GetActiveAsync returns only active branches.
    /// </summary>
    [Fact(DisplayName = "Given branches with different active status When getting active Then should return only active branches")]
    public async Task GetActiveAsync_ShouldReturnOnlyActiveBranches()
    {
        // Arrange
        var branch1 = BranchTestData.GenerateValidBranch();
        var branch2 = BranchTestData.GenerateValidBranch();
        branch2.IsActive = false;
        var branch3 = BranchTestData.GenerateValidBranch();
        branch3.IsActive = true;

        await _repository.CreateAsync(branch1);
        await _repository.CreateAsync(branch2);
        await _repository.CreateAsync(branch3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetActiveAsync();

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(b => b.IsActive);
    }

    /// <summary>
    /// Tests that SearchByNameAsync returns branches matching search term.
    /// </summary>
    [Fact(DisplayName = "Given branches with various names When searching by name Then should return matching branches")]
    public async Task SearchByNameAsync_WithSearchTerm_ShouldReturnMatchingBranches()
    {
        // Arrange
        var branch1 = BranchTestData.GenerateValidBranch();
        branch1.Name = "Downtown Main Branch";
        var branch2 = BranchTestData.GenerateValidBranch();
        branch2.Name = "Uptown Main Branch";
        var branch3 = BranchTestData.GenerateValidBranch();
        branch3.Name = "Suburban Office";

        await _repository.CreateAsync(branch1);
        await _repository.CreateAsync(branch2);
        await _repository.CreateAsync(branch3);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.SearchByNameAsync("Main");

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(b => b.Name.Contains("Main"));
    }

    /// <summary>
    /// Tests that UpdateAsync successfully updates branch information.
    /// </summary>
    [Fact(DisplayName = "Given valid branch updates When updating Then should save changes")]
    public async Task UpdateAsync_WithValidBranch_ShouldUpdateBranch()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Name = "Original Branch";
        branch.Address = "Original Address";
        branch.Phone = "(11) 77777-7777";
        branch.Manager = "Original Manager";
        branch.IsActive = true;

        var createdBranch = await _repository.CreateAsync(branch);
        await _context.SaveChangesAsync();

        // Act
        createdBranch.Name = "Updated Branch";
        createdBranch.Address = "Updated Address";
        createdBranch.Manager = "Updated Manager";
        await _repository.UpdateAsync(createdBranch);
        await _context.SaveChangesAsync();

        // Assert
        var updatedBranch = await _repository.GetByIdAsync(createdBranch.Id);
        updatedBranch!.Name.Should().Be("Updated Branch");
        updatedBranch.Address.Should().Be("Updated Address");
        updatedBranch.Manager.Should().Be("Updated Manager");
    }

    /// <summary>
    /// Tests that DeleteAsync successfully removes branch.
    /// </summary>
    [Fact(DisplayName = "Given existing branch When deleting Then should remove branch")]
    public async Task DeleteAsync_WithExistingBranch_ShouldRemoveBranch()
    {
        // Arrange
        var branch = BranchTestData.GenerateValidBranch();
        branch.Name = "Branch To Delete";
        branch.Address = "Delete Address";
        branch.Manager = "Delete Manager";
        branch.IsActive = true;

        var createdBranch = await _repository.CreateAsync(branch);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(createdBranch.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedBranch = await _repository.GetByIdAsync(createdBranch.Id);
        deletedBranch.Should().BeNull();
    }



    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}