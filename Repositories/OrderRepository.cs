using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
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

    public async Task<IList<OrdersViewModel>> List()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .Include(o => o.CustomerOrders)
                .ThenInclude(co => co.Customer)
            .ToListAsync();
            
            var view = orders.Select(o => new OrdersViewModel{
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                OrderProducts = o.OrderProducts.Select( o => new OrderProductsViewModel{
                    ProductName = o.Product.Name,
                    QuantityOfPackages = o.QuantityOfPackages
                }).ToList()
            }).ToList();

        return view;

    }
}
