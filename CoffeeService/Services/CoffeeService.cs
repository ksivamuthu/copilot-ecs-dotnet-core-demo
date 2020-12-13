using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

public class CoffeeService
{
    private readonly DynamoDBContext _context;
    public CoffeeService(DynamoDBContext context) {
        _context = context;
    }

    public  async Task<List<Coffee>> GetAll() {
        var result = this._context.ScanAsync<Coffee>(new List<ScanCondition>());
        return await result.GetRemainingAsync();
    }

    public  async Task<Coffee> GetById(string coffeeId) {
       return await this._context.LoadAsync<Coffee>(coffeeId);        
    }

    public async Task<Coffee> Create(Coffee coffee) {
        await _context.SaveAsync(coffee);
        return await _context.LoadAsync<Coffee>(coffee.CoffeeId);
    }

    public  async Task<Coffee> Update(string coffeeId, Coffee coffee) {
       var retrievedCoffee = await this._context.LoadAsync<Coffee>(coffeeId);        
       retrievedCoffee.CoffeeName = coffee.CoffeeName;
       await _context.SaveAsync(retrievedCoffee);
       return await _context.LoadAsync<Coffee>(coffeeId);
    }

    public  async Task Delete(string coffeeId) {
       await this._context.DeleteAsync<Coffee>(coffeeId);              
    }
}