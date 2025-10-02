# Business Rules

## 📋 Regras de Negócio

### **🛒 Sales (Vendas)**

#### **Criação de Venda**

- **Obrigatórios**: Cliente, Filial, pelo menos 1 item
- **Validações**:
  - Cliente deve estar ativo
  - Filial deve existir e estar operacional
  - Cada item deve ter quantidade > 0
  - Preço unitário deve ser > 0
  - Produto deve estar ativo no catálogo

#### **Cálculo de Descontos**

- **Regra de Quantidade**: A partir de 4 unidades do mesmo produto
  - 4-9 unidades: 10% de desconto
  - 10-19 unidades: 20% de desconto
  - 20+ unidades: Não é permitido (negócio não aceita)
- **Aplicação**: Desconto aplicado apenas no item específico
- **Exceção**: Produtos em promoção não acumulam desconto por quantidade

#### **Limite de Itens**

- **Máximo**: 20 produtos diferentes por venda
- **Validação**: Sistema deve impedir adição além do limite
- **Comportamento**: Exibir mensagem clara ao usuário

#### **Estados da Venda**

```csharp
public enum SaleStatus
{
    Draft,      // Rascunho - em construção
    Confirmed,  // Confirmada - processada
    Cancelled   // Cancelada - não válida
}
```

### **👤 Customers (Clientes)**

#### **Cadastro de Cliente**

- **Campos Obrigatórios**:
  - Nome completo (mín. 3 caracteres)
  - Email (formato válido e único)
  - Documento (CPF/CNPJ válido)
  - Telefone (formato brasileiro)

#### **Validações de Documento**

- **CPF**: Validação por dígitos verificadores
- **CNPJ**: Validação por dígitos verificadores
- **Unicidade**: Não permitir documentos duplicados

#### **Status do Cliente**

- **Active**: Cliente pode realizar compras
- **Inactive**: Cliente bloqueado temporariamente
- **Blocked**: Cliente bloqueado permanentemente

### **🏢 Branches (Filiais)**

#### **Operação da Filial**

- **Horário de Funcionamento**: 08:00 às 18:00
- **Validação**: Vendas só podem ser registradas em horário comercial
- **Exceção**: Administradores podem registrar fora do horário

#### **Tipos de Filial**

```csharp
public enum BranchType
{
    Store,      // Loja física
    Warehouse,  // Depósito
    Online      // E-commerce
}
```

### **📦 Products (Produtos)**

#### **Gerenciamento de Estoque**

- **Controle**: Por filial e produto
- **Validação**: Não permitir venda sem estoque suficiente
- **Reserva**: Estoque é reservado na criação da venda

#### **Categorias de Produto**

```csharp
public enum ProductCategory
{
    Beverage,   // Bebida
    Food,       // Alimento
    Snack,      // Petisco
    Other       // Outros
}
```

#### **Regras de Precificação**

- **Preço Base**: Definido por produto
- **Promoções**: Podem sobrescrever preço base
- **Desconto Máximo**: 30% sobre preço original

### **🔐 Security & Authorization**

#### **Níveis de Acesso**

```csharp
public enum UserRole
{
    Customer,    // Cliente - apenas compras próprias
    Employee,    // Funcionário - operações básicas
    Manager,     // Gerente - operações da filial
    Admin        // Administrador - acesso total
}
```

#### **Políticas de Segurança**

- **Senhas**: Mínimo 8 caracteres, maiúscula, minúscula, número
- **JWT**: Expiração em 24 horas
- **Refresh Token**: Válido por 30 dias
- **Rate Limiting**: 100 requests por minuto por IP

### **📊 Business Logic Examples**

#### **Cálculo de Desconto por Quantidade**

```csharp
public decimal CalculateQuantityDiscount(int quantity, decimal unitPrice)
{
    return quantity switch
    {
        >= 20 => throw new BusinessException("Quantidade máxima excedida"),
        >= 10 => unitPrice * 0.20m, // 20% desconto
        >= 4 => unitPrice * 0.10m,  // 10% desconto
        _ => 0m                      // Sem desconto
    };
}
```

#### **Validação de Horário Comercial**

```csharp
public bool IsWithinBusinessHours(DateTime saleDate)
{
    var time = saleDate.TimeOfDay;
    return time >= TimeSpan.FromHours(8) &&
           time <= TimeSpan.FromHours(18);
}
```

#### **Cálculo de Total da Venda**

```csharp
public decimal CalculateTotalAmount(IEnumerable<SaleItem> items)
{
    return items.Sum(item =>
    {
        var subtotal = item.Quantity * item.UnitPrice;
        var discount = CalculateQuantityDiscount(item.Quantity, item.UnitPrice);
        return subtotal - (discount * item.Quantity);
    });
}
```

### **🔄 Business Workflows**

#### **Fluxo de Criação de Venda**

1. **Validar Cliente**: Verificar se está ativo
2. **Validar Filial**: Confirmar operação
3. **Validar Produtos**: Verificar disponibilidade
4. **Calcular Descontos**: Aplicar regras de quantidade
5. **Calcular Total**: Somar itens com descontos
6. **Criar Venda**: Persistir no banco
7. **Atualizar Estoque**: Reduzir quantidades
8. **Disparar Evento**: SaleCreated para notificações

#### **Fluxo de Cancelamento**

1. **Validar Permissão**: Verificar autorização
2. **Verificar Status**: Apenas vendas confirmadas
3. **Reverter Estoque**: Devolver quantidades
4. **Atualizar Status**: Marcar como cancelada
5. **Disparar Evento**: SaleCancelled para auditoria

### **⚠️ Exception Handling**

#### **Exceções de Negócio**

```csharp
public class BusinessException : Exception
{
    public string ErrorCode { get; }
    public BusinessException(string code, string message)
        : base(message) => ErrorCode = code;
}

// Exemplos de uso:
throw new BusinessException("MAX_QUANTITY_EXCEEDED",
    "Quantidade máxima de 20 unidades excedida");

throw new BusinessException("CUSTOMER_INACTIVE",
    "Cliente inativo não pode realizar compras");
```

#### **Códigos de Erro Padronizados**

- `CUSTOMER_INACTIVE`: Cliente inativo
- `BRANCH_CLOSED`: Filial fechada
- `INSUFFICIENT_STOCK`: Estoque insuficiente
- `MAX_QUANTITY_EXCEEDED`: Quantidade máxima excedida
- `MAX_ITEMS_EXCEEDED`: Máximo de itens excedido
- `INVALID_DISCOUNT`: Desconto inválido
