using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class CustomerOrder
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public Order Order { get; set; }
    public Customer Customer { get; set; }
}
