using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.ViewModels;
using bageri.api.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace bageri.api;

public class ProductRepositories : IProductRepository
{
    private readonly DataContext _context;
    public ProductRepositories(DataContext context)
    {
        _context = context;
        
    }
    public async Task<bool> AddProduct(AddProductViewModel model)
    {
        var view = new Product
        {
            Name = model.Name,
            PricePackage = model.PricePackage,
            WeightInKg = model.WeightInKg,
            AmountInPackage = model.AmountInPackage
        };
        
        await _context.AddAsync(view);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<GetProductViewModel> FindProduct(int id)
    {
        var p = await _context.Products
            .Where(p => p.ProductId == id)
            .Include(p => p.ProductPreparations)
            .SingleOrDefaultAsync();

        if (p is null)
        {
            throw new Exception($"Finns ingen produkt med id {id}");
        }

        var view = new GetProductViewModel
        {
                ProductId = p.ProductId,
                ProductName = p.Name,
                WeightInKg = p.WeightInKg,
                PricePackage = p.PricePackage,
                AmountInPackage = p.AmountInPackage
        };

        IList<ProductPreparationViewModel> prep = [];
        foreach (var productPrep in p.ProductPreparations)
        {
            var prepView = new ProductPreparationViewModel
            {
                ExpiryDate = productPrep.ExpiryDate,
                PreparationDate = productPrep.PreparationDate
            };
            prep.Add(prepView);
        }
        view.ProductPreparations = prep;
        return view;
    }

    public async Task<IList<ProductsViewModel>> ListAllProducts()
    {
         var products = await _context.Products.ToListAsync();

         IList<ProductsViewModel> response = [];

         foreach(var p in products)
         {
            var view = new ProductsViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.Name,
                WeightInKg = p.WeightInKg,
                PricePackage = p.PricePackage,
                AmountInPackage = p.AmountInPackage
            };

            response.Add(view);
         }
         return response;
    }

    public async Task<bool> Update(int id, decimal price)
    {
        var prod = await _context.Products.SingleOrDefaultAsync(c => c.ProductId == id);

        if(prod is null)
        {
            throw new Exception($"Produkt med id {id} existerar inte");
        }
        prod.PricePackage = price;

        return await _context.SaveChangesAsync() >0;
    }
}
