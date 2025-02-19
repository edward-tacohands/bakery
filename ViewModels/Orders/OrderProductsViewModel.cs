using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Orders;

public class OrderProductsViewModel : BaseOrderProductsViewModel
{
    public decimal PricePackage { get; set; }
    public int AmountInPackage { get; set; }
    public decimal PricePerPiece { get; set; }
    public int QuantityOfPieces { get; set; }
    public decimal TotalPriceForProduct { get; set; }

}
