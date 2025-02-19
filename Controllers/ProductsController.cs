using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repo;
    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpGet()]
    public async Task<ActionResult>ListAllProducts()
    {
        return Ok(new{success = true, data = await _repo.ListAllProducts()});
    }

    [HttpGet("{id}")]
    public async Task<ActionResult>FindProduct(int id)
    {

        try
        {
            return Ok(new{success = true, data = await _repo.FindProduct(id)});
        }
        catch (Exception ex)
        {
            return NotFound(new{success = false, message = ex.Message});
        }
    }

    [HttpPost()]
    public async Task<ActionResult>AddProduct(AddProductViewModel model)
    {
        var product = await _repo.AddProduct(model);
        if(product)
        {
            return StatusCode (201);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPatch("{id}/{price}")]
    public async Task<ActionResult>UpdateProductPrice(int id, decimal price)
    {
        var newPrice = await _repo.Update(id, price);

        if(newPrice)
        {
            return StatusCode(204);
        }
        else
        {
            return BadRequest();
        }
    }
    
}
