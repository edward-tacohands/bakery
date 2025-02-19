using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Address;
using bageri.api.ViewModels.Orders;

namespace bageri.api.ViewModels.Customers;

public class FindCustomerViewModel : BaseCustomerViewModel
{
    public IList<AddressViewModel> Addresses { get; set; }
    public ContactInformationsViewModel Contact { get; set; }
    public IList<OrdersViewModel> Orders { get; set; }
}
