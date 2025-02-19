using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; }
    public IList<OrderProduct> OrderProducts { get; set; }
    public IList<CustomerOrder> CustomerOrders { get; set; }
}
