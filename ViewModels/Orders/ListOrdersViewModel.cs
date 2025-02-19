using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Orders;

public class ListOrdersViewModel
{
    public string OrderNumber { get; set; } 
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalPriceForOrder { get; set; }
    public IList<OrderProductsViewModel> OrderProducts { get; set; }
}
