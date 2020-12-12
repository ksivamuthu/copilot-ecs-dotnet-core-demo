using System;
using Amazon.DynamoDBv2.DataModel;

public class Coffee
{
    [DynamoDBHashKey]
    public string CoffeeId { get; set; }

    public string CoffeeName { get; set; }
}