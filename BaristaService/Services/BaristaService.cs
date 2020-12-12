using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
public class BaristaService
{
    private readonly IDynamoDBContext _context;
    public BaristaService(IDynamoDBContext context) {
        _context = context;
    }
}