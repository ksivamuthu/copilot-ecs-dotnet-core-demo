using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CoffeeController : ControllerBase
{
    private readonly CoffeeService _coffeeService;
    public CoffeeController(CoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
    }

    [HttpGet]
    public List<Coffee> GetAll()
    {
        return _coffeeService.GetAll();
    }
}