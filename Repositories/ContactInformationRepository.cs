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

    public async Task<ContactInformation> Add(AddContactViewModel model)
    {
        var cc = await _context.CustomerContactInformations.FirstOrDefaultAsync(
            c => c.ContactInformationId == model.ContactInformationId);

        if (cc is null)
        {
            cc = new CustomerContactInformation
            {
                ContactInformationId = model.ContactInformationId,
                CustomerId = model.CustomerId
            };
            await _context.CustomerContactInformations.AddAsync(cc);
        }
        
        var contact = await _context.ContactInformations.FirstOrDefaultAsync(
            c => c.Email.ToLower().Trim() == model.Email.ToLower().Trim());
        
        if (contact is null)
        {
            contact = new ContactInformation
            {
                ContactPerson = model.ContactPerson,
                Email = model.Email.ToLower().Trim(),
                PhoneNumber = model.PhoneNumber.Replace(" ", "").Trim(),
                CustomerContactInformation = cc
            };
            await _context.ContactInformations.AddAsync(contact);
        }

        await _context.SaveChangesAsync();
        return contact;
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
