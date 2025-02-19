using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Product;

public class NewBatchViewModel
{
    public int ProductId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime PreparationDate { get; set; }
}
