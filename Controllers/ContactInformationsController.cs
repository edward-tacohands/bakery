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
    private readonly IContactInformationRepository _repo;
    public ContactInformationsController(IContactInformationRepository repo)
    {
        _repo = repo;
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Update(int id, UpdateContactInformationsViewModel model)
    {
        var result = await _repo.Update(id, model);
        if (result)
        {
            return StatusCode(204);
        }
        else
        {
            return BadRequest();
        }
    }
}
