using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Customers;
using bageri.api.ViewModels.Product;

namespace bageri.api.ViewModels.Orders;

public class AddOrderViewModel : AddCustomerViewModel
{
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public IList<GetProductForOrdersViewModel> Products { get; set; }

}
