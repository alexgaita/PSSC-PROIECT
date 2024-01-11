using System.Collections.Immutable;
using Domain.Commands;
using Domain.Models;
using Domain.Workflows;
using Microsoft.AspNetCore.Mvc;
using TakeCommand.API.Models;

namespace TakeCommand.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromServices]PlaceOrderWorkflow placeOrderWorkflow,[FromBody]InputOrder inputOrder)
    {
        
        var products = inputOrder.Products
            .Select(p => new UnvalidatedOrderProduct(p.Id, p.Quantity))
            .ToImmutableList();
        
        var command = new PlaceOrderCommand(inputOrder.Address, inputOrder.Email, products);

        var smth = await placeOrderWorkflow.ExecuteAsync(command);
        
        return Ok();
    }
    
    
}