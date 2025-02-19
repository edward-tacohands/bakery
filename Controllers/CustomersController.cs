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
    private readonly ICustomerRepository _repo;
    
    public CustomersController(ICustomerRepository repo)
    {
            _repo = repo;
       
    }

    [HttpGet()]
    public async Task<ActionResult>ListAllCustomers(){

        var customers = await _repo.List();

        return Ok(new{success = true, data = customers});
    }

    [HttpGet("{id}")]
    public async Task<ActionResult>FindCustomer(int id){

        try
        {
            return Ok(new{success = true, data = await _repo.Find(id)});
        }
        catch (Exception ex)
        {
            return NotFound(new{success = false, message = ex.Message});
        }
    }

    [HttpPost()]
    public async Task<ActionResult>AddCustomer(AddCustomerForRepositoryViewModel model)
    {
        var result = await _repo.Add(model);
        if(result)
        {
            return StatusCode(201);
        }
        else
        {
            return BadRequest();
        }
    }

  
}
