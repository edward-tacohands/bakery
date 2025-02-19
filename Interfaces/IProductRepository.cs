using bageri.api.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace bageri.api;

public interface IProductRepository
{
    public Task<IList<ProductsViewModel>> ListAllProducts();
    public Task<GetProductViewModel> FindProduct(int id);
    public Task<bool> AddProduct(AddProductViewModel model);
    public Task<bool> Update(int id, decimal price);
}
