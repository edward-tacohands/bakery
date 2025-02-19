using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bageri.api.Entities;

public class Customer
{
    public int CustomerId { get; set; } 
    public string Name { get; set; }
    public CustomerContactInformation CustomerContactInformation { get; set; }
    public IList<CustomerAddress> CustomerAddresses { get; set; }
    public IList<CustomerOrder> CustomerOrders { get; set; }
}

