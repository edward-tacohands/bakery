using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Helpers;
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
    private readonly IAddressRepository _repo;
    public CustomerRepository(DataContext context, IAddressRepository repo)
    {
        _repo = repo;
        _context = context;
        
    }
    public async Task<bool> Add(AddCustomerViewModel model)
    {
        try
        {
            if(await _context.Customers.FirstOrDefaultAsync(c => c.Name.ToLower().Trim()
            == model.CustomerName.ToLower().Trim()) is not null)
            {
            throw new Exception("Kunden finns redan");
            }

            var customer = new Customer
            {
                Name = model.CustomerName
            };

            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
            
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
                var address = await _repo.Add(add);

                await _context.CustomerAddresses.AddAsync(new CustomerAddress
                {
                    Address = address,
                    Customer = customer
                });
            }
            return await _context.SaveChangesAsync() >0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
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

            if(customer is null)
            {
                throw new BageriException($"Ingen kund med Id: {id} finns i systemet");
            }

            var view = new FindCustomerViewModel
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.Name
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

            var orders = customer.CustomerOrders.Select(c => new BaseOrdersViewModel{
                OrderNumber = c.Order.OrderNumber,
                OrderDate = c.Order.OrderDate,
                BaseOrderProducts = c.Order.OrderProducts.Select(p => new BaseOrderProductsViewModel{
                    ProductName = p.Product.Name,
                    QuantityOfPackages = p.QuantityOfPackages
                }).ToList()
                
            });

            view.Addresses = addresses.ToList();
            view.Contact = contact;
            view.Orders = orders.ToList();

            return view;
        }
        catch (BageriException ex)
        {
            throw new Exception(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception($"Någonting gick fel {ex.Message}");
        }
    }

    public async Task<IList<ListCustomersViewModel>> List()
    {
        try
        {
            var customers = await _context.Customers
                .Include(c => c.CustomerContactInformation)
                    .ThenInclude(c => c.ContactInformation)
                .ToListAsync();

            var view = customers.Select(c => new ListCustomersViewModel { 
                CustomerId = c.CustomerId,
                CustomerName = c.Name,
                ContactPerson = c.CustomerContactInformation.ContactInformation.ContactPerson,
                ContactInformationId = c.CustomerContactInformation.ContactInformation.ContactInformationId,
                Email = c.CustomerContactInformation.ContactInformation.Email,
                PhoneNumber = c.CustomerContactInformation.ContactInformation.PhoneNumber
            });

            return view.ToList();          
        }
        catch (Exception ex)
        {
            throw new Exception($"Ett fel inträffade {ex.Message}");
        }
    }

}
