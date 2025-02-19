using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Address;

namespace bageri.api.ViewModels.Customers;

public class ListCustomersViewModel : BaseCustomerViewModel
{
    public string ContactPerson { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
