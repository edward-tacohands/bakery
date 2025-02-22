using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Interfaces;
using bageri.api.ViewModels;
using bageri.api.ViewModels.ContactInformation;
using Microsoft.AspNetCore.Mvc;

namespace bageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactInformationsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ContactInformationsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Update(int id, UpdateContactInformationsViewModel model)
    {
        try
        {
            if(await _unitOfWork.ContactInformationRepository.Update(id, model))
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
            return NotFound(new{success = false, message = ex.Message});
        }
        
    }
}
