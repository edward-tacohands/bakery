using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.Helpers;
using bageri.api.Interfaces;
using bageri.api.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace bageri.api.Repositories;

public class ProductPreparationRepository : IProductPreparationRepository
{
    private readonly DataContext _context;
    public ProductPreparationRepository(DataContext context)
    {
        _context = context;
        
    }
    public async Task<bool> Add(NewBatchViewModel model)
    {
        try
        {
            if(await _context.Products.SingleOrDefaultAsync(p=> p.ProductId == model.ProductId) is null)
            {
                throw new BageriException ($"Produkten med id {model.ProductId} finns inte i systemet");
            }
            if(model.ExpiryDate < model.PreparationDate)
            {
                throw new BageriException("Produktens utgångsdatum kan inte vara före bakdatumet");
            }
            
            var newBatch = new ProductPreparation
            {
                ProductId = model.ProductId,
                ExpiryDate = model.ExpiryDate,
                PreparationDate = model.PreparationDate
            };

            await _context.ProductPreparations.AddAsync(newBatch);
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
