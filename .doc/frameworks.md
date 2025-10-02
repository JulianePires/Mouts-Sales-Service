# Frameworks

## 🏗️ Frameworks e Bibliotecas Utilizados

### **Arquitetura**

#### **Clean Architecture**
- **Princípio**: Separação de responsabilidades em camadas
- **Benefício**: Testabilidade, manutenibilidade e flexibilidade
- **Implementação**: Domain, Application, Infrastructure, WebApi

#### **Domain-Driven Design (DDD)**
- **Princípio**: Modelagem rica do domínio de negócio
- **Benefício**: Código que reflete o negócio real
- **Implementação**: Entities, Value Objects, Services, Repositories

### **Padrões de Design**

#### **Repository Pattern**
```csharp
public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale);
    Task<Sale?> GetByIdAsync(Guid id);
    // ... outros métodos
}
```
- **Finalidade**: Abstração da camada de acesso a dados
- **Benefício**: Testabilidade e flexibilidade

#### **Unit of Work**
- **Implementação**: Através do DbContext do Entity Framework
- **Benefício**: Transações consistentes e controle de mudanças

#### **Dependency Injection**
```csharp
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
```
- **Container**: Microsoft.Extensions.DependencyInjection nativo
- **Benefício**: Baixo acoplamento e alta testabilidade

### **Frameworks de Desenvolvimento**

#### **Entity Framework Core 8**
```csharp
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder) { }
}
```
- **Recursos**: Code-First, Migrations, LINQ
- **Provider**: Npgsql para PostgreSQL
- **Benefícios**: Produtividade e type-safety

#### **ASP.NET Core 8**
```csharp
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase { }
```
- **Recursos**: Minimal APIs, Model Binding, Routing
- **Middleware**: Pipeline de processamento customizável

### **Frameworks de Testes**

#### **xUnit**
```csharp
[Fact(DisplayName = "Given valid sale When creating Then should return created sale")]
public async Task CreateSale_ValidData_ReturnsCreatedSale() { }
```
- **Características**: Testes paralelos, fixtures, theory
- **Integração**: Suporte nativo ao .NET

#### **FluentAssertions**
```csharp
result.Should().NotBeNull();
result.Id.Should().NotBe(Guid.Empty);
```
- **Benefício**: Assertions mais legíveis e expressivas

### **Frameworks de Validação**

#### **FluentValidation**
```csharp
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
    }
}
```
- **Características**: Validação fluente e composável
- **Integração**: Pipeline de request do ASP.NET Core

### **Frameworks de Mapeamento**

#### **AutoMapper**
```csharp
CreateMap<Sale, SaleDto>()
    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name));
```
- **Finalidade**: Mapeamento entre DTOs e entidades
- **Benefício**: Redução de código boilerplate

### **Frameworks de Logging**

#### **Microsoft.Extensions.Logging**
```csharp
_logger.LogInformation("Sale created with ID: {SaleId}", sale.Id);
```
- **Providers**: Console, File, Application Insights
- **Structured Logging**: Suporte a logs estruturados

### **Frameworks de Documentação**

#### **Swagger/OpenAPI**
```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sales API", Version = "v1" });
});
```
- **Recursos**: Documentação automática e interface interativa
- **Integração**: Geração automática a partir de controllers

### **Frameworks de Segurança**

#### **JWT Bearer Authentication**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { });
```
- **Implementação**: Tokens stateless para APIs
- **Configuração**: Chaves simétricas e validação automática