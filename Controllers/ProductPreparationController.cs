using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Interfaces;
using bageri.api.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductPreparationController : ControllerBase
{
    private readonly IProductPreparationRepository _repo;
    
    public ProductPreparationController(IProductPreparationRepository repo)
    {
        _repo = repo;
        
    }

    [HttpPost()]
    public async Task<ActionResult>AddNewBatchOfProduct(NewBatchViewModel model)
    {
        // if(await _context.Products.FirstOrDefaultAsync(p=> p.ProductId == model.ProductId) is null)
        // {
        //     return BadRequest(new{success = false, message =$"Produkten med {model.ProductId} finns inte i systemet"});
        // }
        
        // var newBatch = new ProductPreparation
        // {
        //     ProductId = model.ProductId,
        //     ExpiryDate = model.ExpiryDate,
        //     PreparationDate = model.PreparationDate
        // };

        // await _context.ProductPreparations.AddAsync(newBatch);
        // await _context.SaveChangesAsync();

        var newBatch = await _repo.Add(model);
        if(newBatch)
        {
            return StatusCode(201);
        }
        else
        {
            return BadRequest();
        }
    }
}
