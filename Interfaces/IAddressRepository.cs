using bageri.api.Entities;
using bageri.api.ViewModels.Address;

namespace bageri.api;

public interface IAddressRepository
{
    public Task<Address>Add(AddAddressViewModel model);
}
