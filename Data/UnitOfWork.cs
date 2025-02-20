using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Interfaces;
using bageri.api.Repositories;

namespace bageri.api.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private readonly IAddressRepository _repo;
    public UnitOfWork(DataContext context, IAddressRepository repo)
    {
        _repo = repo;
        _context = context;
    }

    public IAddressRepository AddressRepository => new AddressRepository(_context);

    public IContactInformationRepository ContactInformationRepository => throw new NotImplementedException();

    public ICustomerRepository CustomerRepository => throw new NotImplementedException();

    public IOrderRepository OrderRepository => throw new NotImplementedException();

    public IProductPreparationRepository ProductPreparationRepository => throw new NotImplementedException();

    public IProductRepository ProductRepository => throw new NotImplementedException();

    public ISupplierRepository SupplierRepository => throw new NotImplementedException();

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
