using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Orders;

public class ListOrdersViewModel : BaseOrdersViewModel
{
    public string CustomerName { get; set; }
    public decimal TotalPriceForOrder { get; set; }
}
