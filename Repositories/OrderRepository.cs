using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Helpers;
using bageri.api.Interfaces;
using bageri.api.ViewModels.Address;
using bageri.api.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext _context;
    private readonly IAddressRepository _repo;
    public OrderRepository(DataContext context, IAddressRepository repo)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<bool> Add(AddOrderViewModel model)
    {

        try
        {
    var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == model.CustomerName.ToLower().Trim());

            if (customer is null)
            {
                customer = new Customer
                {
                    Name = model.CustomerName.Trim()
                };
                await _context.Customers.AddAsync(customer);

                foreach (var add in model.Addresses)
                {
                    var address = await _repo.Add(add);

                    await _context.CustomerAddresses.AddAsync(new CustomerAddress{
                        Address = address,
                        Customer = customer
                    });
                }

                var newContact = await _context.ContactInformations.FirstOrDefaultAsync(c =>
                    c.Email.ToLower().Trim() == model.Contact.Email.ToLower().Trim() ||
                    c.ContactPerson.ToLower().Trim() == model.Contact.ContactPerson.ToLower().Trim() ||
                    c.PhoneNumber.Replace(" ", "").Trim() == model.Contact.PhoneNumber.Replace(" ", "").Trim());

                if (newContact is null)
                {
                    newContact = new ContactInformation
                    {
                        ContactPerson = model.Contact.ContactPerson.ToLower().Trim(),
                        Email = model.Contact.Email.ToLower().Trim(),
                        PhoneNumber = model.Contact.PhoneNumber.Replace(" ", "").Trim()
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
                    throw new BageriException("Du har angivit en produkt ett id som inte existerar");
                }

                newOrder.OrderProducts.Add(new OrderProduct
                {
                    QuantityOfPackages = product.QuantityOfPackages,
                    ProductId = product.ProductId
                });
            }

            return await _context.SaveChangesAsync() >0;            
        }
        catch (BageriException ex)
        {
            throw new Exception(ex.Message);
        }
        catch(Exception ex)
        {
            throw new Exception($"Någonting gick fel {ex.Message}");
        }
    }

    public async Task<ListOrdersViewModel> Find(string orderNumber)
    {
        try
        {
            var order = await _context.Orders
                .Where(o => o.OrderNumber.ToLower().Trim() == orderNumber.ToLower().Trim() )
                .Include( o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.CustomerOrders)
                    .ThenInclude(co => co.Customer)
            .SingleOrDefaultAsync();

            if(order is null)
            {
                throw new BageriException($"Ordernumret som du angivit {orderNumber}, finns inte i systemet");
            }

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
        catch (BageriException ex)
        {
            throw new Exception(ex.Message);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<ListOrdersViewModel>> Find(DateTime orderDate)
    {
        try
        {
            var orders = await _context.Orders
                .Where(o => o.OrderDate.Date == orderDate.Date)
                .Include( o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.CustomerOrders)
                    .ThenInclude(co => co.Customer)        
            .ToListAsync();

            if(!orders.Any())
            {
                throw new BageriException($"Ingen order hittades under {orderDate}");
            }

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
        catch (BageriException ex)
        {
            throw new Exception(ex.Message);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<ListOrdersViewModel>> List()
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception($"Ett fel inträffade {ex.Message}");
        }
            // var orders = await _context.Orders
            //     .Include(o => o.OrderProducts)
            //         .ThenInclude(op => op.Product)
            //     .Include(o => o.CustomerOrders)
            //         .ThenInclude(co => co.Customer)
            //     .ToListAsync();
                
            //     var view = orders.Select(o => new ListOrdersViewModel{
            //         CustomerName = o.CustomerOrders.Select(co => co.Customer.Name).SingleOrDefault(),
            //         OrderNumber = o.OrderNumber,
            //         OrderDate = o.OrderDate,
            //         TotalPriceForOrder = o.OrderProducts.Sum(op => op.Product.PricePackage * op.QuantityOfPackages),
            //         OrderProducts = o.OrderProducts.Select( o => new OrderProductsViewModel{
            //             ProductName = o.Product.Name,
            //             QuantityOfPackages = o.QuantityOfPackages,
            //             PricePackage = o.Product.PricePackage,
            //             AmountInPackage = o.Product.AmountInPackage,
            //             PricePerPiece = o.Product.PricePackage / o.Product.AmountInPackage,
            //             QuantityOfPieces = o.Product.AmountInPackage * o.QuantityOfPackages,
            //             TotalPriceForProduct = o.Product.PricePackage * o.QuantityOfPackages
            //         }).ToList()
            //     }).ToList();
            // return view;
    }

    
}
