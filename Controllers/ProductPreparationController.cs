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
    
    private readonly IUnitOfWork _unitOfWork;
    
    public ProductPreparationController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
    }

    [HttpPost()]
    public async Task<ActionResult>AddNewBatchOfProduct(NewBatchViewModel model)
    {
        try
        {
            if(await _unitOfWork.ProductPreparationRepository.Add(model))
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
}
