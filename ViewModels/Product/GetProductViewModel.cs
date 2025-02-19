using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Product;

public class GetProductViewModel : ProductsViewModel
{
    public IList<ProductPreparationViewModel> ProductPreparations { get; set; }
}
