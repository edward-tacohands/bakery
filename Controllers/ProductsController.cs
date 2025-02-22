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
using SQLitePCL;

namespace bageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
    }

    [HttpGet()]
    public async Task<ActionResult>ListAllProducts()
    {
        try
        {
            return Ok(new{success = true, data = await _unitOfWork.ProductRepository.ListAllProducts()});
        }
        catch (Exception ex)
        {
            return NotFound(new{success = false, message = $"Gick inte att hitta produkter {ex.Message}"});
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult>FindProduct(int id)
    {

        try
        {
            return Ok(new{success = true, data = await _unitOfWork.ProductRepository.FindProduct(id)});
        }
        catch (Exception ex)
        {
            return NotFound(new{success = false, message = ex.Message});
        }
    }

    [HttpPost()]
    public async Task<ActionResult>AddProduct(AddProductViewModel model)
    {
        try
        {
            if(await _unitOfWork.ProductRepository.AddProduct(model))
            {
                if(_unitOfWork.HasChanges())
                {
                    await _unitOfWork.Complete();
                }
                return StatusCode(201);
            }
            else{
                return BadRequest();
            }            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/{price}")]
    public async Task<ActionResult>UpdateProductPrice(int id, decimal price)
    {
        try
        {
            if(await _unitOfWork.ProductRepository.Update(id, price))
            {
                if(_unitOfWork.HasChanges())
                {
                    await _unitOfWork.Complete();
                }
                return StatusCode(204);
            }
            else{
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
 
    }
    
}
