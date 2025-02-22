using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Interfaces;
using bageri.api.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    
    private readonly IUnitOfWork _unitOfWork;
    
    public OrdersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
    }

    [HttpGet()]
    public async Task<ActionResult> ListAllOrders()
    {
        var result = await _unitOfWork.OrderRepository.List();

        return Ok(new { success = true, data = result });
    }

    [HttpGet("ordernumber/{orderNumber}")]
    public async Task<ActionResult> FindOrderByOrderNumber(string orderNumber)
    {
        var result = await _unitOfWork.OrderRepository.Find(orderNumber);

        return Ok(new { success = true, data = result });
    }

    [HttpGet("orderdate")]
    public async Task<ActionResult> FindOrderByOrderDate([FromQuery] DateTime orderDate)
    {
        
        var result = await _unitOfWork.OrderRepository.Find(orderDate);

        return Ok(new { success = true, data = result });
    }

    [HttpPost()]
    public async Task<ActionResult> AddOrder(AddOrderViewModel model)
    {
        if(await _unitOfWork.OrderRepository.Add(model))
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

}
