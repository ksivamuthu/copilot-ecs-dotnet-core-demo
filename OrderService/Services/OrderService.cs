using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.EventBridge;

public class OrderService
{
    private readonly IDynamoDBContext _context;
    private readonly IAmazonEventBridge _eventBridge;
    private readonly EventBusConfig _config;
    public OrderService(IDynamoDBContext context, IAmazonEventBridge eventBridge, EventBusConfig config) {
        _context = context;
        _eventBridge = eventBridge;
        _config = config;
    }

    public async Task<Order> Create(Order order) {
        await _context.SaveAsync(order);
        await _eventBridge.PutEventsAsync(new Amazon.EventBridge.Model.PutEventsRequest() {
            Entries = new System.Collections.Generic.List<Amazon.EventBridge.Model.PutEventsRequestEntry>() {
                new Amazon.EventBridge.Model.PutEventsRequestEntry() {
                    EventBusName = _config.EventBusName, 
                    Detail = JsonSerializer.Serialize(order),
                    DetailType = "OrderCreated",
                    Source = "OrderService"
                }
            }
        });

        return await _context.LoadAsync<Order>(order.OrderId, new DynamoDBContextConfig { ConsistentRead = true });
    }

    public async Task<Order> GetById(string orderId) 
    {
        return await _context.LoadAsync<Order>(orderId);
    }
}