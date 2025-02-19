using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels;

public class ProductPreparationViewModel
{
    public DateTime ExpiryDate { get; set; }
    public DateTime PreparationDate { get; set; }
}
