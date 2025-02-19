using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bageri.api.ViewModels.Address;

namespace bageri.api.ViewModels.Customers;

public class AddCustomerViewModel
{
    public string Name { get; set; }
    public string DeliveryAddress { get; set; }
    public string DeliveryPostalCode { get; set; }
    public string DeliveryCity { get; set; }
    public string InvoiceAddress { get; set; }
    public string InvoicePostalCode { get; set; }
    public string InvoiceCity { get; set; } 
    public string ContactPerson { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}
