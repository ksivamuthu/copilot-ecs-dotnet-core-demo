using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
public class OrderService
{
    private readonly IDynamoDBContext _context;
    public OrderService(IDynamoDBContext context) {
        _context = context;
    }

    public async Task<Order> Create(Order order) {
        await _context.SaveAsync(order);
        return await _context.LoadAsync<Order>(order.OrderId, new DynamoDBContextConfig { ConsistentRead = true });
    }
}