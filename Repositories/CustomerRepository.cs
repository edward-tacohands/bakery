using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Interfaces;
using bageri.api.ViewModels;
using bageri.api.ViewModels.Address;
using bageri.api.ViewModels.Customers;
using bageri.api.ViewModels.Orders;
using bageri.api.ViewModels.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _context;
    private readonly IAddressRepository _aRepo;
    public CustomerRepository(DataContext context, IAddressRepository aRepo)
    {
        _aRepo = aRepo;
        _context = context;
        
    }
    public async Task<bool> Add(AddCustomerForRepositoryViewModel model)
    {
        if(await _context.Customers.FirstOrDefaultAsync(c => c.Name.ToLower().Trim()
            == model.Name.ToLower().Trim()) is not null)
        {
            throw new Exception("Kunden finns redan");
        }

        var customer = new Customer
        {
            Name = model.Name
        };

        await _context.AddAsync(customer);
        await _context.SaveChangesAsync();

        /***************************************************/
        
        var contact = await _context.ContactInformations.FirstOrDefaultAsync(c=> c.Email.ToLower().Trim() 
            == model.Contact.Email.ToLower().Trim());

        if(contact is not null)
        {
            throw new Exception("Kunden finns redan");
        }
        else
        {
            contact = new ContactInformation
            {
                ContactPerson = model.Contact.ContactPerson,
                Email = model.Contact.Email,
                PhoneNumber = model.Contact.PhoneNumber
            };

            await _context.ContactInformations.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        var cci = new CustomerContactInformation
        {
            CustomerId = customer.CustomerId,
            ContactInformationId = contact.ContactInformationId
        };

        await _context.CustomerContactInformations.AddAsync(cci);

        foreach (var add in model.Addresses)
        {
            var address = await _aRepo.Add(add);

            await _context.CustomerAddresses.AddAsync(new CustomerAddress
            {
                Address = address,
                Customer = customer
            });
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<FindCustomerViewModel> Find(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Where(c => c.CustomerId == id)
                .Include(c => c.CustomerAddresses)
                    .ThenInclude(c => c.Address)
                    .ThenInclude(c => c.AddressType)
                .Include(c => c.CustomerAddresses)
                    .ThenInclude(c => c.Address)
                    .ThenInclude(c => c.PostalAddress)
                .Include(c => c.CustomerContactInformation)
                    .ThenInclude(c=> c.ContactInformation)
                .Include(c => c.CustomerOrders)
                    .ThenInclude(c => c.Order)
                    .ThenInclude(c => c.OrderProducts)
                    .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync();

            var view = new FindCustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name
            };

            var addresses = customer.CustomerAddresses.Select(c => new AddressViewModel
            {
                AddressLine = c.Address.AddressLine,
                City = c.Address.PostalAddress.City,
                PostalCode = c.Address.PostalAddress.PostalCode,
                AddressType = c.Address.AddressType.Value
            });

            var contact = new ContactInformationsViewModel
            {
                ContactInformationId = customer.CustomerContactInformation.ContactInformation.ContactInformationId,
                ContactPerson = customer.CustomerContactInformation.ContactInformation.ContactPerson,
                Email = customer.CustomerContactInformation.ContactInformation.Email,
                PhoneNumber = customer.CustomerContactInformation.ContactInformation.PhoneNumber
            };

            var orders = customer.CustomerOrders.Select(c => new OrdersViewModel{
                OrderNumber = c.Order.OrderNumber,
                OrderDate = c.Order.OrderDate,
                OrderProducts = c.Order.OrderProducts.Select(p => new OrderProductsViewModel{
                    ProductName = p.Product.Name,
                    QuantityOfPackages = p.QuantityOfPackages
                }).ToList()
                
            });

            view.Addresses = addresses.ToList();
            view.Contact = contact;
            view.Orders = orders.ToList();

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<ListCustomersViewModel>> List()
    {
        var response = await _context.Customers
            .Include(c => c.CustomerContactInformation)
                .ThenInclude(c => c.ContactInformation)
            .ToListAsync();

        var customers = response.Select(c => new ListCustomersViewModel { 
            CustomerId = c.CustomerId,
            Name = c.Name,
            ContactPerson = c.CustomerContactInformation.ContactInformation.ContactPerson,
            Email = c.CustomerContactInformation.ContactInformation.Email,
            PhoneNumber = c.CustomerContactInformation.ContactInformation.PhoneNumber
        });

        return customers.ToList();
    }

}
