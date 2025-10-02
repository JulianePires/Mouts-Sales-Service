# Project Structure

## 📁 Estrutura do Projeto

### **Visão Geral da Arquitetura**

```
📁 backend/
├── 📁 src/                              # Código fonte principal
│   ├── 🎯 Ambev.DeveloperEvaluation.Domain/      # Camada de domínio
│   ├── 🔧 Ambev.DeveloperEvaluation.Application/  # Camada de aplicação
│   ├── 🌐 Ambev.DeveloperEvaluation.WebApi/      # Camada de apresentação
│   ├── 💾 Ambev.DeveloperEvaluation.ORM/         # Camada de infraestrutura
│   ├── 🔌 Ambev.DeveloperEvaluation.IoC/         # Inversão de controle
│   └── 🛠️ Ambev.DeveloperEvaluation.Common/      # Utilitários compartilhados
├── 📁 tests/                           # Testes automatizados
├── 📁 .doc/                            # Documentação do projeto
└── 🐳 docker-compose.yml               # Configuração de containers
```

### **🎯 Domain Layer** (`Ambev.DeveloperEvaluation.Domain`)

```
📁 Domain/
├── 📁 Entities/                    # Entidades de domínio
│   ├── Sale.cs                     # Entidade principal de venda
│   ├── SaleItem.cs                 # Itens da venda
│   ├── Customer.cs                 # Cliente
│   ├── Branch.cs                   # Filial
│   ├── Product.cs                  # Produto
│   └── User.cs                     # Usuário
├── 📁 Repositories/                # Interfaces de repositório
│   └── ISaleRepository.cs          # Contrato do repositório de vendas
├── 📁 Services/                    # Serviços de domínio
│   └── IDiscountService.cs         # Serviço de cálculo de desconto
├── 📁 Events/                      # Eventos de domínio
│   ├── SaleCreated.cs             # Evento de venda criada
│   ├── SaleModified.cs            # Evento de venda modificada
│   └── SaleCancelled.cs           # Evento de venda cancelada
├── 📁 ValueObjects/               # Objetos de valor
├── 📁 Enums/                      # Enumerações
├── 📁 Exceptions/                 # Exceções de domínio
└── 📁 Validation/                 # Validações específicas
```

**Responsabilidades**:
- Regras de negócio e lógica de domínio
- Entidades com comportamentos ricos
- Contratos (interfaces) para serviços

### **🔧 Application Layer** (`Ambev.DeveloperEvaluation.Application`)

```
📁 Application/
├── 📁 Sales/                      # Casos de uso de vendas
│   ├── CreateSale/               # Criar venda
│   ├── GetSale/                  # Obter venda
│   ├── UpdateSale/               # Atualizar venda
│   └── DeleteSale/               # Cancelar venda
├── 📁 DTOs/                      # Data Transfer Objects
│   ├── SaleDto.cs               # DTO de venda
│   ├── SaleItemDto.cs           # DTO de item de venda
│   └── CreateSaleDto.cs         # DTO para criação
├── 📁 Mappings/                  # Perfis do AutoMapper
│   └── SaleMappingProfile.cs    # Mapeamento de vendas
└── 📁 Validators/                # Validadores FluentValidation
    └── CreateSaleValidator.cs    # Validador de criação
```

**Responsabilidades**:
- Orquestração de casos de uso
- Coordenação entre domínio e infraestrutura
- Transformação de dados (DTOs)

### **🌐 WebApi Layer** (`Ambev.DeveloperEvaluation.WebApi`)

```
📁 WebApi/
├── 📁 Controllers/               # Controllers da API
│   ├── SalesController.cs       # Endpoints de vendas
│   ├── CustomersController.cs   # Endpoints de clientes
│   └── ProductsController.cs    # Endpoints de produtos
├── 📁 Middleware/               # Middlewares customizados
│   ├── ExceptionMiddleware.cs   # Tratamento de exceções
│   └── LoggingMiddleware.cs     # Logging de requests
├── 📁 Filters/                  # Filtros de ação
├── 📁 Mappings/                 # Mapeamentos específicos da API
├── Program.cs                   # Ponto de entrada da aplicação
├── appsettings.json            # Configurações da aplicação
└── 📁 Properties/              # Propriedades do projeto
```

**Responsabilidades**:
- Endpoints da API REST
- Serialização/deserialização JSON
- Autenticação e autorização
- Documentação Swagger

### **💾 ORM Layer** (`Ambev.DeveloperEvaluation.ORM`)

```
📁 ORM/
├── 📁 Repositories/             # Implementações de repositório
│   └── SaleRepository.cs       # Repositório de vendas
├── 📁 Mapping/                 # Configurações EF Core
│   ├── SaleConfiguration.cs    # Mapeamento de venda
│   ├── SaleItemConfiguration.cs # Mapeamento de item
│   ├── CustomerConfiguration.cs # Mapeamento de cliente
│   ├── BranchConfiguration.cs  # Mapeamento de filial
│   └── ProductConfiguration.cs # Mapeamento de produto
├── 📁 Migrations/              # Migrações do banco
│   ├── 20241014_InitialMigrations.cs
│   ├── 20251002_AddSaleEntities.cs
│   └── DefaultContextModelSnapshot.cs
└── DefaultContext.cs           # DbContext principal
```

**Responsabilidades**:
- Acesso a dados com Entity Framework
- Configurações de mapeamento
- Migrações de banco de dados

### **🔌 IoC Layer** (`Ambev.DeveloperEvaluation.IoC`)

```
📁 IoC/
├── 📁 ModuleInitializers/       # Inicializadores de módulos
│   ├── InfrastructureModuleInitializer.cs
│   ├── ApplicationModuleInitializer.cs
│   └── WebApiModuleInitializer.cs
├── DependencyResolver.cs       # Resolvedor principal
└── IModuleInitializer.cs      # Interface de inicialização
```

**Responsabilidades**:
- Configuração de injeção de dependência
- Registro de serviços por módulo
- Configuração de lifetime dos objetos

### **🛠️ Common Layer** (`Ambev.DeveloperEvaluation.Common`)

```
📁 Common/
├── 📁 Security/                # Utilitários de segurança
│   ├── JwtTokenGenerator.cs    # Geração de tokens JWT
│   └── PasswordHasher.cs       # Hash de senhas
├── 📁 Validation/              # Validações comuns
│   └── ValidationResult.cs     # Resultado de validação
├── 📁 Logging/                 # Configurações de log
└── 📁 Extensions/              # Métodos de extensão
    └── StringExtensions.cs     # Extensões para string
```

**Responsabilidades**:
- Utilitários compartilhados
- Funcionalidades transversais
- Extensões e helpers

### **🧪 Tests** (`tests/`)

```
📁 tests/
├── 📁 Unit/                    # Testes unitários
│   ├── 📁 Domain/              # Testes de domínio
│   ├── 📁 Application/         # Testes de aplicação
│   └── 📁 ORM/                 # Testes de repositório
│       └── SaleRepositoryTests.cs # 14 testes implementados
├── 📁 Integration/             # Testes de integração
│   └── 📁 API/                 # Testes de API
└── 📁 Functional/              # Testes funcionais
```

**Responsabilidades**:
- Verificação de comportamento
- Garantia de qualidade
- Documentação viva do código

### **📄 Documentation** (`.doc/`)

```
📁 .doc/
├── overview.md                 # Visão geral do projeto
├── tech-stack.md              # Stack tecnológico
├── frameworks.md              # Frameworks utilizados
├── project-structure.md       # Estrutura do projeto
├── business-rules.md          # Regras de negócio
└── deployment.md              # Guia de deploy
```

**Responsabilidades**:
- Documentação técnica
- Guias de uso e configuração
- Decisões arquiteturais