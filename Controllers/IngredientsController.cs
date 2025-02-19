using Microsoft.AspNetCore.Mvc;
using bageri.api.Data;
using Microsoft.EntityFrameworkCore;
using bageri.api.Entities;
using bageri.api.ViewModels;

namespace bageri.api.Controllers;
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly DataContext _context;
        public IngredientsController(DataContext context)
        {
            this._context = context;
        }
          
        [HttpGet()]
        public async Task<ActionResult> ListAllIngredients()
        {
            var result = await _context.Ingredients
                .Include(p => p.SupplierIngredients)
                .Select(p => new{
                    p.Name,
                    p.PricePerKg,
                    SupplierInformation = p.SupplierIngredients
                        .Select(sp => new{
                            SupplierName = sp.Supplier.Name,
                            sp.Supplier.SupplierContactInformation.ContactInformation.ContactPerson,
                            Phone = sp.Supplier.SupplierContactInformation.ContactInformation.PhoneNumber,
                            sp.Supplier.SupplierContactInformation.ContactInformation.Email,
                            SupplierAddresses = sp.Supplier.SupplierAddresses
                                .Select(sa => new {
                                    sa.Address.AddressLine,
                                    sa.Address.PostalAddress.City,
                                    sa.Address.PostalAddress.PostalCode,
                                    sa.Address.AddressType.Value
                                })
                        })
                })
                .ToListAsync();

            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> FindIngredient(int id){
            var result = await _context.Ingredients
                .Include( sp => sp.SupplierIngredients)
                .Select( p => new {
                    p.IngredientId,
                    p.Name,
                    p.ItemNumber,
                    p.PricePerKg,
                    SupplierInformation = p.SupplierIngredients
                        .Select(sp => new{
                            SupplierName = sp.Supplier.Name,
                            sp.Supplier.SupplierContactInformation.ContactInformation.ContactPerson,
                            Phone = sp.Supplier.SupplierContactInformation.ContactInformation.PhoneNumber,
                            sp.Supplier.SupplierContactInformation.ContactInformation.Email,
                            SupplierAddresses = sp.Supplier.SupplierAddresses
                                .Select(sa => new {
                                    sa.Address.AddressLine,
                                    sa.Address.PostalAddress.City,
                                    sa.Address.PostalAddress.PostalCode,
                                    sa.Address.AddressType.Value
                                })
                        })
                        
                })
                .SingleOrDefaultAsync(p => p.IngredientId == id);
                
            if( result != null){
                return Ok(new { success = true, data = result });
            }
            else{
                return NotFound( new { success = false, message = $"Ingen ingrediens med id {id} kunde hittas"});
            }
        }

        [HttpGet("ingredientname/{name}")]
        public async Task<ActionResult> FindProductByName(string name)
        {
            var result = await _context.Ingredients
                .Where(p => p.Name.ToLower() == name.ToLower())
                .Include(s => s.SupplierIngredients)                
                .Select(p => new
                {
                    p.Name,
                    p.PricePerKg,
                    SupplierInformation = p.SupplierIngredients
                        .Select(sp => new{
                            SupplierName = sp.Supplier.Name,
                            sp.Supplier.SupplierContactInformation.ContactInformation.ContactPerson,
                            Phone = sp.Supplier.SupplierContactInformation.ContactInformation.PhoneNumber,
                            sp.Supplier.SupplierContactInformation.ContactInformation.Email,
                            SupplierAddresses = sp.Supplier.SupplierAddresses
                                .Select(sa => new {
                                    sa.Address.AddressLine,
                                    sa.Address.PostalAddress.City,
                                    sa.Address.PostalAddress.PostalCode,
                                    sa.Address.AddressType.Value
                                })
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
        public async Task<ActionResult> AddIngredient(int supplierId, AddIngredientViewModel model){
            var s = await _context.Suppliers
                .Include(sp => sp.SupplierIngredients)
                .FirstOrDefaultAsync( sp => sp.SupplierId == supplierId);   

            if (s == null){
                return NotFound(new { success = false, message = $"Leverantören med id {supplierId} existerar inte"});
            } 

            var exists = await _context.SupplierIngredients
                .Include(sp => sp.Ingredient)
                .Where(sp => sp.SupplierId == supplierId && sp.Ingredient.ItemNumber == model.ItemNumber)
                .FirstOrDefaultAsync();
            if (exists != null){
                return BadRequest( new { success = false, message = $"Produkten med artikelnumret {model.ItemNumber} finns redan registrerat hos leverantören"});
            }

            var ingredient = new Ingredient{
                ItemNumber = model.ItemNumber,
                Name = model.Name,
                PricePerKg = model.PricePerKg
            };

            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();

            var supplierIngredient = new SupplierIngredient{
                SupplierId = supplierId,
                IngredientId = ingredient.IngredientId
            };

            await _context.SupplierIngredients.AddAsync(supplierIngredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(FindIngredient), new { id = ingredient.IngredientId}, new {
                ingredient.ItemNumber,
                ingredient.Name,
                Supplier = new {
                    SupplierName = supplierIngredient.Supplier.Name
                }
            });   
        }

        [HttpPatch("{id}/{price}")]
        public async Task<ActionResult>UpdatePrice(int id, decimal price){
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(p => p.IngredientId == id);
            if (ingredient ==  null) return BadRequest(new {success = false, message = $"Ingen produkt med id {id} kunde hittas"});
            
            ingredient.PricePerKg = price;
            try {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex){
                return StatusCode(500, ex.Message);
            }
            return NoContent();
       
        }
    }
