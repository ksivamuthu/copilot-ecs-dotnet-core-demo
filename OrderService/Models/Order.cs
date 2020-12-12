using System;
using Amazon.DynamoDBv2.DataModel;

public class Order
{
    [DynamoDBHashKey]
    public string OrderId { get; set; }
    public string CoffeeId { get; set; }
    public string CoffeeName { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
}