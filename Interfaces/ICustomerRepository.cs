using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Customers;

namespace bageri.api.Interfaces;

public interface ICustomerRepository
{
    public Task<IList<ListCustomersViewModel>>List();
    public Task<FindCustomerViewModel>Find(int id);
    public Task<bool>Add(AddCustomerViewModel model);
}
