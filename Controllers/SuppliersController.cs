using bageri.api.Data;
using bageri.api.Entities;
using bageri.api.ViewModels.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace bageri.api.Controllers;

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
        public async Task<ActionResult>ListSuppliers(){
            var result = await _context.Suppliers
                .Include(s => s.SupplierIngredients)
                .Select(s => new{
                    SupplierName = s.Name,
                    IngredientInformation = s.SupplierIngredients
                        .Select(sp => new{
                            sp.Ingredient.Name,
                            sp.Ingredient.PricePerKg
                        })
                })
                .ToListAsync();

            return Ok(new { success = true, data = result });
        }

        [HttpGet("{name}")]
        public async Task<ActionResult>FindSupplierByName(string name){
            var result = await _context.Suppliers
                .Where(s => s.Name.ToLower() == name.ToLower())
                .Include(s => s.SupplierIngredients)
                .Select(s => new{
                    SupplierName = s.Name,
                    IngredientInformation = s.SupplierIngredients
                        .Select(sp => new{
                            sp.Ingredient.ItemNumber,
                            sp.Ingredient.Name,
                            sp.Ingredient.PricePerKg
                        })
                })
                .ToListAsync();

                if(result.Any()){
                    return Ok(new { success = true, data = result});
                } else{
                    return NotFound( new { success = false, message = $"Leverantören med namnet: {name} kunde inte hittas"});
                }
        }

        


    }
