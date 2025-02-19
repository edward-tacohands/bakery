using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.ViewModels.Orders;

public class OrderProductsViewModel
{
    public string ProductName { get; set; }
    public int QuantityOfPackages { get; set; }
    public decimal PricePackage { get; set; }
    public int AmountInPackage { get; set; }
    public decimal PricePerPiece { get; set; }
    public int QuantityOfPieces { get; set; }
    public decimal TotalPriceForProduct { get; set; }

}
