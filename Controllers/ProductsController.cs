using Microsoft.AspNetCore.Mvc;
using bageri.api.Data;
using Microsoft.EntityFrameworkCore;
using bageri.api.Entities;
using bageri.api.ViewModels;

namespace bageri.api.Controllers;
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        public ProductsController(DataContext context)
        {
            this._context = context;
        }
          
        [HttpGet()]
        public async Task<ActionResult> ListAllProducts()
        {
            var result = await _context.Products
                .Include(p => p.SupplierProducts)
                .Select(p => new{
                    p.ProductName,
                    p.PricePerKg,
                    SupplierInformation = p.SupplierProducts
                        .Select(sp => new{
                            SupplierName = sp.Supplier.Name,
                            sp.Supplier.ContactInformation.ContactPerson,
                            Phone = sp.Supplier.ContactInformation.PhoneNumber,
                            sp.Supplier.ContactInformation.Email,
                            sp.Supplier.SupplierAddress.Address.Street,
                            sp.Supplier.SupplierAddress.Address.StreetNumber,
                            sp.Supplier.SupplierAddress.Address.PostCode,
                            sp.Supplier.SupplierAddress.Address.City
                        })
                })
                .ToListAsync();

            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> FindProduct(int id){
            var result = await _context.Products
                .Include( sp => sp.SupplierProducts)
                .Select( p => new {
                    p.ProductId,
                    p.ProductName,
                    p.ItemNumber,
                    p.PricePerKg,
                    SupplierInformation = p.SupplierProducts
                        .Select(sp => new {
                            SupplierName = sp.Supplier.Name,
                            sp.Supplier.ContactInformation.ContactPerson,
                            Phone = sp.Supplier.ContactInformation.PhoneNumber,
                            sp.Supplier.ContactInformation.Email,
                            sp.Supplier.SupplierAddress.Address.Street,
                            sp.Supplier.SupplierAddress.Address.StreetNumber,
                            sp.Supplier.SupplierAddress.Address.PostCode,
                            sp.Supplier.SupplierAddress.Address.City
                        })
                        
                })
                .SingleOrDefaultAsync(p => p.ProductId == id);
                
            if( result != null){
                return Ok(new { success = true, data = result });
            }
            else{
                return NotFound( new { success = false, message = $"Ingen produkt med id {id} kunde hittas"});
            }
        }

        [HttpGet("productname/{name}")]
        public async Task<ActionResult> FindProductByName(string name)
        {
            var result = await _context.Products
                .Where(p => p.ProductName.ToLower() == name.ToLower())
                .Include(s => s.SupplierProducts)                
                .Select(p => new
                {
                    p.ProductName,
                    p.PricePerKg,
                    SupplierInformation = p.SupplierProducts
                        .Select(sp => new{
                            SupplierName = sp.Supplier.Name,
                            sp.Supplier.ContactInformation.ContactPerson,
                            Phone = sp.Supplier.ContactInformation.PhoneNumber,
                            sp.Supplier.ContactInformation.Email,
                            sp.Supplier.SupplierAddress.Address.Street,
                            sp.Supplier.SupplierAddress.Address.StreetNumber,
                            sp.Supplier.SupplierAddress.Address.PostCode,
                            sp.Supplier.SupplierAddress.Address.City
                        })
                })
                .ToListAsync();

                if (result.Any()){
                    return Ok(new { success = true, data = result});
                }else {
                    return NotFound( new { success = false, message = $"Ingen produkt med namnet: {name} kunde hittas"});
                }
        }

        [HttpPost("{supplierId}")]
        public async Task<ActionResult> AddProduct(int supplierId, AddProductViewModel model){
            var s = await _context.Suppliers
                .Include(sp => sp.SupplierProducts)
                .FirstOrDefaultAsync( sp => sp.SupplierId == supplierId);   

            if (s == null){
                return NotFound(new { success = false, message = $"Leverantören med id {supplierId} existerar inte"});
            } 

            var exists = await _context.SupplierProducts
                .Include(sp => sp.Product)
                .Where(sp => sp.SupplierId == supplierId && sp.Product.ItemNumber == model.ItemNumber)
                .FirstOrDefaultAsync();
            if (exists != null){
                return BadRequest( new { success = false, message = $"Produkten med artikelnumret {model.ItemNumber} fanns redan registrerat hos leverantören"});
            }

            var product = new Product{
                ItemNumber = model.ItemNumber,
                ProductName = model.ProductName,
                PricePerKg = model.PricePerKg
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var supplierProduct = new SupplierProduct{
                SupplierId = supplierId,
                ProductId = product.ProductId
            };

            await _context.SupplierProducts.AddAsync(supplierProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(FindProduct), new { id = product.ProductId}, new {
                product.ItemNumber,
                product.ProductName,
                Supplier = new {
                    SupplierName = supplierProduct.Supplier.Name
                }
            });   
        }

        [HttpPatch("{id}/{price}")]
        public async Task<ActionResult>UpdatePrice(int id, decimal price){
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product ==  null) return BadRequest(new {success = false, message = $"Ingen produkt med id {id} kunde hittas"});
            
            product.PricePerKg = price;
            try {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex){
                return StatusCode(500, ex.Message);
            }
            return NoContent();
       
        }
    }
