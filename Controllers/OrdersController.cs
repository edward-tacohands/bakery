using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Interfaces;
using bageri.api.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repo;
    
    public OrdersController(IOrderRepository repo)
    {
        _repo = repo;
        
    }

    [HttpGet()]
    public async Task<ActionResult> ListAllOrders()
    {
        var result = await _repo.List();

        return Ok(new { success = true, data = result });
    }

    // [HttpGet("ordernumber/{orderNumber}")]
    // public async Task<ActionResult> FindOrderByOrderNumber(string orderNumber)
    // {
    //     var order = await _context.Orders
    //         .Where(o => o.OrderNumber.ToLower().Trim() == orderNumber.ToLower().Trim())
    //         .Include(o => o.OrderProducts)
    //         .Include(o => o.CustomerOrders)
    //         .Select(o => new
    //         {
    //             Customer = o.CustomerOrders
    //             .Select(o => new
    //             {
    //                 o.Customer.Name
    //             }),
    //             o.OrderNumber,
    //             o.OrderDate,
    //             OrderProduct = o.OrderProducts
    //             .Select(o => new
    //             {
    //                 o.Product.Name,
    //                 o.Product.PricePackage,
    //                 o.Product.WeightInKg,
    //                 o.QuantityOfPackages,
    //                 QuantityOfPieces = o.QuantityOfPackages * o.Product.AmountInPackage,
    //                 PricePerPiece = o.Product.PricePackage / o.Product.AmountInPackage,
    //                 TotalPriceForProduct = o.Product.PricePackage * o.QuantityOfPackages
    //             }),
    //             TotalPriceForOrder = o.OrderProducts.Sum(op => op.Product.PricePackage * op.QuantityOfPackages)
    //         })
    //     .FirstOrDefaultAsync();

    //     return Ok(new { success = true, data = order });
    // }

    // [HttpGet("orderdate")]
    // public async Task<ActionResult> FindOrderByOrderDate([FromQuery] DateTime orderDate)
    // {
    //     var orders = await _context.Orders
    //         .Where(o => o.OrderDate.Date == orderDate)
    //         .Include(o => o.OrderProducts)
    //         .Include(o => o.CustomerOrders)
    //         .Select(o => new
    //         {
    //             Customer = o.CustomerOrders
    //             .Select(o => new
    //             {
    //                 o.Customer.Name
    //             }),
    //             o.OrderNumber,
    //             o.OrderDate,
    //             OrderProduct = o.OrderProducts
    //             .Select(o => new
    //             {
    //                 o.Product.Name,
    //                 o.Product.PricePackage,
    //                 o.Product.WeightInKg,
    //                 o.QuantityOfPackages,
    //                 QuantityOfPieces = o.QuantityOfPackages * o.Product.AmountInPackage,
    //                 PricePerPiece = o.Product.PricePackage / o.Product.AmountInPackage,
    //                 TotalPriceForProduct = o.Product.PricePackage * o.QuantityOfPackages
    //             }),
    //             TotalPriceForOrder = o.OrderProducts.Sum(op => op.Product.PricePackage * op.QuantityOfPackages)
    //         })
    //     .ToListAsync();

    //     return Ok(new { success = true, data = orders });
    // }

    // [HttpPost()]
    // public async Task<ActionResult> AddOrder(AddOrderViewModel model)
    // {
        
    //     var customer = await _context.Customers
    //         .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == model.Name.ToLower().Trim());

    //     if (customer is null)
    //     {
    //         var deliveryPostalAddress = await _context.PostalAddresses
    //             .FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim() == model.DeliveryPostalCode.Replace(" ", "").Trim());

    //         var deliveryAddress = await _context.Addresses.FirstOrDefaultAsync(
    //             c => c.AddressLine.ToLower().Trim() == model.DeliveryAddress.ToLower().Trim() && c.AddressTypeId == 1);

    //         var invoicePostalAddress = await _context.PostalAddresses
    //             .FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim() == model.InvoicePostalCode.Replace(" ", "").Trim());

    //         var invoiceAddress = await _context.Addresses.FirstOrDefaultAsync(
    //             c => c.AddressLine.ToLower().Trim() == model.InvoiceAddress.ToLower().Trim() && c.AddressTypeId == 3);

    //         if (deliveryPostalAddress is null)
    //         {
    //             deliveryPostalAddress = new PostalAddress
    //             {
    //                 PostalCode = model.DeliveryPostalCode.Replace(" ", "").Trim(),
    //                 City = model.DeliveryCity.Trim()
    //             };
    //             await _context.PostalAddresses.AddAsync(deliveryPostalAddress);
    //         }

    //         if (invoicePostalAddress is null)
    //         {
    //             invoicePostalAddress = new PostalAddress
    //             {
    //                 PostalCode = model.InvoicePostalCode.Replace(" ", "").Trim(),
    //                 City = model.InvoiceCity.Trim()
    //             };
    //             await _context.PostalAddresses.AddAsync(invoicePostalAddress);
    //         }

    //         if (deliveryAddress is null)
    //         {
    //             deliveryAddress = new Address
    //             {
    //                 AddressLine = model.DeliveryAddress.Trim(),
    //                 AddressTypeId = 1,
    //                 PostalAddress = deliveryPostalAddress
    //             };
    //             await _context.Addresses.AddAsync(deliveryAddress);
    //         }

    //         if (invoiceAddress is null)
    //         {
    //             invoiceAddress = new Address
    //             {
    //                 AddressLine = model.InvoiceAddress.Trim(),
    //                 AddressTypeId = 3,
    //                 PostalAddress = invoicePostalAddress
    //             };
    //             await _context.Addresses.AddAsync(invoiceAddress);
    //         }

    //         customer = new Customer
    //         {
    //             Name = model.Name.Trim()
    //         };
    //         await _context.Customers.AddAsync(customer);

    //         await _context.CustomerAddresses.AddRangeAsync(new[]
    //         {
    //             new CustomerAddress { Address = deliveryAddress, Customer = customer },
    //             new CustomerAddress { Address = invoiceAddress, Customer = customer }
    //         });

    //         var newContact = await _context.ContactInformations.FirstOrDefaultAsync(c =>
    //             c.Email.ToLower().Trim() == model.Email.ToLower().Trim() ||
    //             c.ContactPerson.ToLower().Trim() == model.ContactPerson.ToLower().Trim() ||
    //             c.PhoneNumber.Replace(" ", "").Trim() == model.PhoneNumber.Replace(" ", "").Trim());

    //         if (newContact is null)
    //         {
    //             newContact = new ContactInformation
    //             {
    //                 ContactPerson = model.ContactPerson.ToLower().Trim(),
    //                 Email = model.Email.ToLower().Trim(),
    //                 PhoneNumber = model.PhoneNumber.Replace(" ", "").Trim()
    //             };
    //             await _context.ContactInformations.AddAsync(newContact);
    //         }

    //         var customerContact = new CustomerContactInformation
    //         {
    //             ContactInformation = newContact,
    //             Customer = customer
    //         };
    //         await _context.CustomerContactInformations.AddAsync(customerContact);
    //     }

    //     var productIds = model.Products.Select(p => p.ProductId).ToList();
    //     var existingProducts = await _context.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();

    //     var newOrder = new Order
    //     {
    //         OrderDate = model.OrderDate,
    //         OrderNumber = model.OrderNumber,
    //         OrderProducts = []
    //     };

    //     var newCustomerOrder = new CustomerOrder
    //     {
    //         Customer = customer,
    //         Order = newOrder
    //     };

    //     await _context.CustomerOrders.AddAsync(newCustomerOrder);

    //     foreach (var product in model.Products)
    //     {
    //         var p = existingProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
    //         if (p is null)
    //         {
    //             return BadRequest($"Du har angivit ett produkt id som inte existerar");
    //         }

    //         newOrder.OrderProducts.Add(new OrderProduct
    //         {
    //             QuantityOfPackages = product.QuantityOfPackages,
    //             ProductId = product.ProductId
    //         });
    //     }

    //     try
    //     {
    //         await _context.Orders.AddAsync(newOrder);
    //         await _context.SaveChangesAsync();

    //         return NoContent();
    //     }
    //     catch (Exception ex)
    //     {
    //         System.Console.WriteLine(ex.Message);
    //         return BadRequest();
    //     }
    // }

}
