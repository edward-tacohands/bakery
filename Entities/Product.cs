using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal PricePackage { get; set; }
    public decimal WeightInKg { get; set; } 
    public int AmountInPackage { get; set; }
    public IList<OrderProduct> OrderProducts { get; set; }
    public IList<ProductPreparation> ProductPreparations { get; set; }

}
