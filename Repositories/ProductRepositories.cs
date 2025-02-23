using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Helpers;
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
        try
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Name == model.ProductName);
            
            if(prod is not null)
            {
                throw new BageriException($"Produkten {model.ProductName} finns redan i systemet");
            }

            var view = new Product
            {
                Name = model.ProductName,
                PricePackage = model.PricePackage,
                WeightInKg = model.WeightInKg,
                AmountInPackage = model.AmountInPackage
            };
            
            await _context.AddAsync(view);

            return await _context.SaveChangesAsync() >0;         

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<GetProductViewModel> FindProduct(int id)
    {
        try
        {
            var p = await _context.Products
                .Where(p => p.ProductId == id)
                .Include(p => p.ProductPreparations)
                .SingleOrDefaultAsync();

            if (p is null)
            {
                throw new BageriException($"Finns ingen produkt med id {id}");
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
        catch (BageriException ex)
        {
            throw new Exception(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }        

    }

    public async Task<IList<ProductsViewModel>> ListAllProducts()
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception($"Ett fel inträffade {ex.Message}");
        }

    }

    public async Task<bool> Update(int id, decimal price)
    {
        try
        {
            var prod = await _context.Products.SingleOrDefaultAsync(c => c.ProductId == id);

            if(prod is null)
            {
                throw new BageriException($"Produkt med id {id} existerar inte");
            }
            prod.PricePackage = price;

            return await _context.SaveChangesAsync() >0;            
        }
        catch (BageriException ex)
        {
            throw new Exception(ex.Message);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}
