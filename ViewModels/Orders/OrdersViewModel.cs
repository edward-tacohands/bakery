using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Orders;

public class OrdersViewModel
{
    public string OrderNumber { get; set; } 
    
    public DateTime OrderDate { get; set; }
    public IList<OrderProductsViewModel> OrderProducts { get; set; }
}
