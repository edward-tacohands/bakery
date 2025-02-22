using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Address;
using bageri.api.ViewModels.ContactInformation;

namespace bageri.api.ViewModels.Customers;

public class AddCustomerViewModel : BaseCustomerViewModel
{
    public IList<AddAddressViewModel> Addresses { get; set; }
    public BaseContactInformationsViewModel Contact { get; set; }
}
