using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Interfaces;

public interface IUnitOfWork
{
    IAddressRepository AddressRepository { get; }
    IContactInformationRepository ContactInformationRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IOrderRepository OrderRepository { get; }
    IProductPreparationRepository ProductPreparationRepository { get; }
    IProductRepository ProductRepository { get; }

    Task <bool> Complete ();
    bool HasChanges();
}
