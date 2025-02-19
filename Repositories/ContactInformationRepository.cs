using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Interfaces;
using bageri.api.ViewModels;
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
}
