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

    public Task<IList<ListOrdersViewModel>> Find(DateTime orderDate)
    {
        throw new NotImplementedException();
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
