using bageri.api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace bageri.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly DataContext _context;
        public SuppliersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult> ListSuppliers(){
            var result = await _context.Suppliers
                .Include(s => s.SupplierProducts)
                .Select(s => new{
                    SupplierName = s.Name,
                    ProductInformation = s.SupplierProducts
                        .Select(sp => new{
                            sp.Product.ProductName,
                            sp.Product.PricePerKg
                        })
                })
                .ToListAsync();

            return Ok(new { success = true, data = result });
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> FindSupplierByName(string name){
            var result = await _context.Suppliers
                .Where(s => s.Name.ToLower() == name.ToLower())
                .Include(s => s.SupplierProducts)
                .Select(s => new{
                    SupplierName = s.Name,
                    ProductInformation = s.SupplierProducts
                        .Select(sp => new{
                            sp.Product.ItemNumber,
                            sp.Product.ProductName,
                            sp.Product.PricePerKg
                        })
                })
                .ToListAsync();
                return Ok(new { success = true, data = result});
        }
    }
}