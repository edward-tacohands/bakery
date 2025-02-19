using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Address;

namespace bageri.api.ViewModels.Customers;

public class AddCustomerForRepositoryViewModel : BaseCustomerViewModel
{
    public IList<AddAddressViewModel> Addresses { get; set; }
    public AddContactViewModel Contact { get; set; }
}
