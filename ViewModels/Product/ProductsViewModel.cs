using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Product;

public class ProductsViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal PricePackage { get; set; }
    public decimal WeightInKg { get; set; }
    public int AmountInPackage { get; set; }    

}
