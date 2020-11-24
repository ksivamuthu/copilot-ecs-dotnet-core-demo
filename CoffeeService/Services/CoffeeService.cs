using System.Collections.Generic;

public class CoffeeService
{
    public List<Coffee> GetAll() 
    {
        return new List<Coffee> {
            new Coffee() { CoffeeId = "cappucino", CoffeeName = "Cappucino" },
            new Coffee() { CoffeeId = "latte", CoffeeName = "Latte" },
            new Coffee() { CoffeeId = "mocha", CoffeeName = "Mocha" },
            new Coffee() { CoffeeId = "americano", CoffeeName = "Americano" },
            new Coffee() { CoffeeId = "macchiato", CoffeeName = "Macchiato" },
            new Coffee() { CoffeeId = "frappe", CoffeeName = "Frappe" },
            new Coffee() { CoffeeId = "corretto", CoffeeName = "Corretto" },
            new Coffee() { CoffeeId = "affogato", CoffeeName = "Affogato" },
            new Coffee() { CoffeeId = "filtercoffee", CoffeeName = "Filter Coffee" },
        };
    }
}