# Azure Service Bus Configuration - Queue Mode

This project has been configured to use Azure Service Bus **queues** instead of topics due to service plan limitations.

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "ServiceBus": "Endpoint=sb://your-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=your-key"
  },
  "ServiceBus": {
    "QueueName": "domain-events-queue"
  }
}
```

### Environment Variables (Alternative)
```bash
export ConnectionStrings__ServiceBus="Endpoint=sb://your-namespace.servicebus.windows.net/;..."
export ServiceBus__QueueName="domain-events-queue"
```

## Azure CLI Setup

### 1. Create Service Bus Namespace (if not exists)
```bash
az servicebus namespace create \
  --resource-group your-resource-group \
  --name your-servicebus-namespace \
  --location eastus \
  --sku Standard
```

### 2. Create Queue
```bash
az servicebus queue create \
  --resource-group your-resource-group \
  --namespace-name your-servicebus-namespace \
  --name domain-events-queue \
  --max-delivery-count 5 \
  --default-message-time-to-live "P14D" \
  --enable-duplicate-detection true
```

### 3. Get Connection String
```bash
az servicebus namespace authorization-rule keys list \
  --resource-group your-resource-group \
  --namespace-name your-servicebus-namespace \
  --name RootManageSharedAccessKey \
  --query primaryConnectionString \
  --output tsv
```

## Queue vs Topic Differences

### Queues (Current Implementation)
- ✅ **Supported in Basic/Standard tiers**
- ✅ Point-to-point messaging
- ✅ Single consumer per message
- ✅ FIFO ordering (with sessions)
- ✅ Competitive consumers pattern

### Topics (Not available in current plan)
- ❌ Requires Premium tier
- ❌ Publish-subscribe messaging
- ❌ Multiple subscribers per message
- ❌ Message filtering and routing

## Event Flow

```
Domain Event → Event Handler → Azure Service Bus Queue → External Consumers
     ↓
Cache Invalidation (Redis)
```

## Event Types Published

1. **SaleCreated** - When a new sale is created
2. **SaleModified** - When a sale is updated
3. **SaleCancelled** - When a sale is cancelled
4. **ItemCancelled** - When individual items are cancelled

## Message Format

```json
{
  "id": "guid",
  "version": 1,
  "occurredOn": "2023-10-03T20:45:30.123Z",
  "aggregateId": "guid",
  // Event-specific properties...
}
```

## Message Properties

- **Subject**: Event type name (e.g., "SaleCreated")
- **MessageId**: Event ID
- **ContentType**: "application/json"
- **EventType**: Custom property for filtering
- **EventVersion**: Event version for compatibility
- **OccurredOn**: ISO timestamp

## Error Handling

- **Max Delivery Count**: 5 attempts
- **Dead Letter Queue**: Automatic for failed messages
- **Message TTL**: 14 days
- **Duplicate Detection**: 10-minute window

## Monitoring

Check queue metrics in Azure Portal:
- Message count
- Dead letter message count
- Incoming/outgoing messages per second
- Queue size

## Local Development

For local development without Azure Service Bus:
1. Comment out Service Bus configuration in `appsettings.Development.json`
2. Events will be logged but not published
3. Cache invalidation will still work locally

## Production Considerations

1. **Connection String Security**: Use Azure Key Vault
2. **Queue Scaling**: Monitor queue length and add consumers
3. **Dead Letter Handling**: Implement dead letter processing
4. **Message Ordering**: Use sessions if strict ordering required
5. **Partitioning**: Consider partitioned queues for high throughput