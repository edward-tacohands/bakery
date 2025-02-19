using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class ProductPreparation
{
    public int ProductPreparationId { get; set; }
    public int ProductId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime PreparationDate { get; set; }
    public Product Product { get; set; }
}
