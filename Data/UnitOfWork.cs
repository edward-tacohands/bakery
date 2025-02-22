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

    public IContactInformationRepository ContactInformationRepository => new ContactInformationRepository(_context);

    public ICustomerRepository CustomerRepository => new CustomerRepository(_context, _repo);

    public IOrderRepository OrderRepository => new OrderRepository(_context, _repo);

    public IProductPreparationRepository ProductPreparationRepository => new ProductPreparationRepository(_context);

    public IProductRepository ProductRepository => new ProductRepositories(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
