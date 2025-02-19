using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Orders;

public class BaseOrderProductsViewModel
{
    public string ProductName { get; set; }
    public int QuantityOfPackages { get; set; }
}
