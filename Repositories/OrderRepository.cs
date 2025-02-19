using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Interfaces;
using bageri.api.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext _context;
    public OrderRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> Add(AddOrderViewModel model)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == model.Name.ToLower().Trim());

        if (customer is null)
        {
            var deliveryPostalAddress = await _context.PostalAddresses
                .FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim() == model.DeliveryPostalCode.Replace(" ", "").Trim());

            var deliveryAddress = await _context.Addresses.FirstOrDefaultAsync(
                c => c.AddressLine.ToLower().Trim() == model.DeliveryAddress.ToLower().Trim() && c.AddressTypeId == 1);

            var invoicePostalAddress = await _context.PostalAddresses
                .FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim() == model.InvoicePostalCode.Replace(" ", "").Trim());

            var invoiceAddress = await _context.Addresses.FirstOrDefaultAsync(
                c => c.AddressLine.ToLower().Trim() == model.InvoiceAddress.ToLower().Trim() && c.AddressTypeId == 3);

            if (deliveryPostalAddress is null)
            {
                deliveryPostalAddress = new PostalAddress
                {
                    PostalCode = model.DeliveryPostalCode.Replace(" ", "").Trim(),
                    City = model.DeliveryCity.Trim()
                };
                await _context.PostalAddresses.AddAsync(deliveryPostalAddress);
            }

            if (invoicePostalAddress is null)
            {
                invoicePostalAddress = new PostalAddress
                {
                    PostalCode = model.InvoicePostalCode.Replace(" ", "").Trim(),
                    City = model.InvoiceCity.Trim()
                };
                await _context.PostalAddresses.AddAsync(invoicePostalAddress);
            }

            if (deliveryAddress is null)
            {
                deliveryAddress = new Address
                {
                    AddressLine = model.DeliveryAddress.Trim(),
                    AddressTypeId = 1,
                    PostalAddress = deliveryPostalAddress
                };
                await _context.Addresses.AddAsync(deliveryAddress);
            }

            if (invoiceAddress is null)
            {
                invoiceAddress = new Address
                {
                    AddressLine = model.InvoiceAddress.Trim(),
                    AddressTypeId = 3,
                    PostalAddress = invoicePostalAddress
                };
                await _context.Addresses.AddAsync(invoiceAddress);
            }

            customer = new Customer
            {
                Name = model.Name.Trim()
            };
            await _context.Customers.AddAsync(customer);

            await _context.CustomerAddresses.AddRangeAsync(new[]
            {
                new CustomerAddress { Address = deliveryAddress, Customer = customer },
                new CustomerAddress { Address = invoiceAddress, Customer = customer }
            });

            var newContact = await _context.ContactInformations.FirstOrDefaultAsync(c =>
                c.Email.ToLower().Trim() == model.Email.ToLower().Trim() ||
                c.ContactPerson.ToLower().Trim() == model.ContactPerson.ToLower().Trim() ||
                c.PhoneNumber.Replace(" ", "").Trim() == model.PhoneNumber.Replace(" ", "").Trim());

            if (newContact is null)
            {
                newContact = new ContactInformation
                {
                    ContactPerson = model.ContactPerson.ToLower().Trim(),
                    Email = model.Email.ToLower().Trim(),
                    PhoneNumber = model.PhoneNumber.Replace(" ", "").Trim()
                };
                await _context.ContactInformations.AddAsync(newContact);
            }

            var customerContact = new CustomerContactInformation
            {
                ContactInformation = newContact,
                Customer = customer
            };
            await _context.CustomerContactInformations.AddAsync(customerContact);
        }

        var productIds = model.Products.Select(p => p.ProductId).ToList();
        var existingProducts = await _context.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();

        var newOrder = new Order
        {
            OrderDate = model.OrderDate,
            OrderNumber = model.OrderNumber,
            OrderProducts = []
        };

        var newCustomerOrder = new CustomerOrder
        {
            Customer = customer,
            Order = newOrder
        };

        await _context.CustomerOrders.AddAsync(newCustomerOrder);

        foreach (var product in model.Products)
        {
            var p = existingProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (p is null)
            {
                throw new Exception($"Du har angivit ett produkt id som inte existerar");
            }

            newOrder.OrderProducts.Add(new OrderProduct
            {
                QuantityOfPackages = product.QuantityOfPackages,
                ProductId = product.ProductId
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ListOrdersViewModel> Find(string orderNumber)
    {
        var order = await _context.Orders
            .Where(o => o.OrderNumber.ToLower().Trim() == orderNumber.ToLower().Trim() )
            .Include( o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .Include(o => o.CustomerOrders)
                .ThenInclude(co => co.Customer)
        .SingleOrDefaultAsync();

        var view = new ListOrdersViewModel
        {
            CustomerName = order.CustomerOrders.Select(co => co.Customer.Name).SingleOrDefault(),
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            TotalPriceForOrder = order.OrderProducts.Sum(op => op.Product.PricePackage * op.QuantityOfPackages),
            OrderProducts = order.OrderProducts.Select(o => new OrderProductsViewModel{
                ProductName = o.Product.Name,
                QuantityOfPackages = o.QuantityOfPackages,
                PricePackage = o.Product.PricePackage,
                AmountInPackage = o.Product.AmountInPackage,
                PricePerPiece = o.Product.PricePackage / o.Product.AmountInPackage,
                TotalPriceForProduct = o.Product.PricePackage * o.QuantityOfPackages
            }).ToList()
        };

        return view;
    }

    public async Task<IList<ListOrdersViewModel>> Find(DateTime orderDate)
    {
        var orders = await _context.Orders
            .Where(o => o.OrderDate == orderDate)
            .Include( o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .Include(o => o.CustomerOrders)
                .ThenInclude(co => co.Customer)        
        .ToListAsync();

         var view = orders.Select(o => new ListOrdersViewModel{
                CustomerName = o.CustomerOrders.Select(co => co.Customer.Name).SingleOrDefault(),
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                TotalPriceForOrder = o.OrderProducts.Sum(op => op.Product.PricePackage * op.QuantityOfPackages),
                OrderProducts = o.OrderProducts.Select( o => new OrderProductsViewModel{
                    ProductName = o.Product.Name,
                    QuantityOfPackages = o.QuantityOfPackages,
                    PricePackage = o.Product.PricePackage,
                    AmountInPackage = o.Product.AmountInPackage,
                    PricePerPiece = o.Product.PricePackage / o.Product.AmountInPackage,
                    QuantityOfPieces = o.Product.AmountInPackage * o.QuantityOfPackages,
                    TotalPriceForProduct = o.Product.PricePackage * o.QuantityOfPackages
                }).ToList()
            }).ToList();
            
        return view;
    }

    public async Task<IList<ListOrdersViewModel>> List()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .Include(o => o.CustomerOrders)
                .ThenInclude(co => co.Customer)
            .ToListAsync();
            
            var view = orders.Select(o => new ListOrdersViewModel{
                CustomerName = o.CustomerOrders.Select(co => co.Customer.Name).SingleOrDefault(),
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                TotalPriceForOrder = o.OrderProducts.Sum(op => op.Product.PricePackage * op.QuantityOfPackages),
                OrderProducts = o.OrderProducts.Select( o => new OrderProductsViewModel{
                    ProductName = o.Product.Name,
                    QuantityOfPackages = o.QuantityOfPackages,
                    PricePackage = o.Product.PricePackage,
                    AmountInPackage = o.Product.AmountInPackage,
                    PricePerPiece = o.Product.PricePackage / o.Product.AmountInPackage,
                    QuantityOfPieces = o.Product.AmountInPackage * o.QuantityOfPackages,
                    TotalPriceForProduct = o.Product.PricePackage * o.QuantityOfPackages
                }).ToList()
            }).ToList();
        return view;
    }

    
}
