using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;

namespace bageri.api.Interfaces;

public interface IOrderRepository
{
    public Task<IList<ListOrdersViewModel>>List();
    public Task<ListOrdersViewModel>Find(string orderNumber);
    public Task<IList<ListOrdersViewModel>>Find(DateTime orderDate);
    public Task<bool> Add(AddOrderViewModel model);
    
}
