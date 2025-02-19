using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Product;

public class GetProductForOrdersViewModel
{
    public int ProductId { get; set; }
    public int QuantityOfPackages { get; set; }
}
