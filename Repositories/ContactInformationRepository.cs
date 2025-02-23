using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Helpers;
using bageri.api.Interfaces;
using bageri.api.ViewModels;
using bageri.api.ViewModels.ContactInformation;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Repositories;

public class ContactInformationRepository : IContactInformationRepository
{
    private readonly DataContext _context;
    public ContactInformationRepository(DataContext context)
    {
        _context = context;
    }


    public async Task<bool> Update(int id, UpdateContactInformationsViewModel model)
    {
        try
        {
            var cc = await _context.ContactInformations.FirstOrDefaultAsync(cc => cc.ContactInformationId == id);
            
            if(cc is null)
            {
                throw new BageriException($"Ingen kontaktperson med Id: {id} hittades");
            }

            cc.ContactPerson = model.ContactPerson;
            cc.Email = model.Email;
            cc.PhoneNumber = model.PhoneNumber;    
            return await _context.SaveChangesAsync() >0;      
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
