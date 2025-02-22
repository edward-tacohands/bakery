using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Interfaces;
using bageri.api.Repositories;
using bageri.api.ViewModels.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    
    private readonly IUnitOfWork _unitOfWork;
    
    public CustomersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet()]
    public async Task<ActionResult>ListAllCustomers(){

        try
        {
            var customers = await _unitOfWork.CustomerRepository.List();

            return Ok(new{success = true, data = customers});            
        }
        catch (Exception ex)
        {
            return NotFound($"Gick inte att hitta kunder {ex.Message}");
        }
    }

   [HttpGet("{id}")]
    public async Task<ActionResult>FindCustomer(int id){

        try
        {
            return Ok(new{success = true, data = await _unitOfWork.CustomerRepository.Find(id)});
        }
        catch (Exception ex)
        {
            return NotFound(new{success = false, message = ex.Message});
        }
    }

    [HttpPost()]
    public async Task<ActionResult>AddCustomer(AddCustomerViewModel model)
    {
        try
        {
            if(await _unitOfWork.CustomerRepository.Add(model))
            {
                    if(_unitOfWork.HasChanges())
                    {
                        await _unitOfWork.Complete();
                    }
                    return StatusCode(201);
            }
            else
            {
                    return BadRequest();
            }            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  
}
