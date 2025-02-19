using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.ViewModels.Address;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly DataContext _context;
    public AddressRepository(DataContext context)
    {
        _context = context;
        
    }
    public async Task<Address> Add(AddAddressViewModel model)
    {
        var postalAddress = await _context.PostalAddresses.FirstOrDefaultAsync(
            c=> c.PostalCode.Replace(" ", "").Trim() == model.PostalCode.Replace(" ", "").Trim());

        if(postalAddress is null)
        {
            postalAddress = new PostalAddress
            {
                PostalCode = model.PostalCode.Replace(" ", "").Trim(),
                City = model.City.Trim()
            };
            await _context.PostalAddresses.AddAsync(postalAddress);
        }

        var address = await _context.Addresses.FirstOrDefaultAsync(
            c => c.AddressLine.Trim().ToLower() == model.AddressLine.Trim().ToLower());
        
        if(address is null)
        {
            address = new Address
            {
                AddressLine = model.AddressLine,
                AddressTypeId = (int)model.AddressType,
                PostalAddress = postalAddress
            };
            await _context.Addresses.AddAsync(address);
        }

        await _context.SaveChangesAsync();
        return address;
    }
}
